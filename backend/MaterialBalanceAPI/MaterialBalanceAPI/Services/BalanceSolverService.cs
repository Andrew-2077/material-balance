using Accord.Math.Optimization;
using MaterialBalanceAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Distributions.Univariate;
using Accord.Math;
using System;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialBalanceAPI.Services
{
    public interface IBalanceSolverService
    {
        BalanceResponse Solve(BalanceRequest request);
    }

    public class BalanceSolverService : IBalanceSolverService
    {
        private readonly BalanceOptions _options;

        public BalanceSolverService(IOptions<BalanceOptions> options)
        {
            _options = options.Value;
        }

        public BalanceSolverService() : this(
            Microsoft.Extensions.Options.Options.Create(new BalanceOptions()))
        {

        }

        public BalanceResponse Solve(BalanceRequest request)
        {
            var vars = request.Variables;
            int n = vars.Count;

            // 1. Формируем список уникальных узлов
            var nodes = vars.Select(v => v.SourceId)
                            .Concat(vars.Select(v => v.DestinationId))
                            .Where(id => id != null)
                            .Distinct()
                            .ToList();

            int m = nodes.Count;

            // 2. Строим матрицу Aeq (балансы) и beq
            // Для каждого узла сумма входящих равна сумме исходящих
            double[,] Aeq = new double[m, n];
            double[] beq = new double[m];

            for (int i = 0; i < m; i++)
            {
                string nodeId = nodes[i];
                for (int j = 0; j < n; j++)
                {
                    if (vars[j].DestinationId == nodeId) Aeq[i, j] = 1.0;
                    else if (vars[j].SourceId == nodeId) Aeq[i, j] = -1.0;
                    else Aeq[i, j] = 0.0;
                }
            }

            var varsArray = vars.ToArray();

            // Расчет глобального теста и поиск ошибок
            double gtValue = CalculateGlobalTest(Aeq, varsArray, n);
            Stopwatch sw1 = Stopwatch.StartNew();
            var grossErrorStreams = DetectGrossErrors(Aeq, nodes, varsArray, gtValue);
            sw1.Stop();

            // 3. Формируем H (веса) и f (линейный член)
            double[,] H = new double[n, n];
            double[] f = new double[n];

            for (int i = 0; i < n; i++)
            {
                if (vars[i].IsMeasured && vars[i].Tolerance > 0.0m)
                {
                    double tol = (double)vars[i].Tolerance;
                    double weight = 1.0 / (tol * tol);
                    H[i, i] = weight;
                    f[i] = -weight * (double)vars[i].Measured;
                }
                else
                {
                    H[i, i] = 1e-6;
                    f[i] = 0.0;
                }
            }

            // 4. Ограничения lb (нижние) и ub (верхние)
            double[] lb = vars.Select(v => (double)v.MinBound).ToArray();
            double[] ub = vars.Select(v => (double)v.MaxBound).ToArray();

            // 5. Инициализация солвера (алгоритм Goldfarb-Idnani)
            var objective = new QuadraticObjectiveFunction(H, f);
            var constraints = new List<LinearConstraint>();

            // Добавляем балансовые ограничения (Aeq * x = beq)
            for (int i = 0; i < m; i++)
            {
                double[] row = new double[n];
                for (int j = 0; j < n; j++)
                {
                    row[j] = Aeq[i, j];
                }
                constraints.Add(new LinearConstraint(n)
                {
                    VariablesAtIndices = Enumerable.Range(0, n).ToArray(),
                    CombinedAs = row,
                    ShouldBe = ConstraintType.EqualTo,
                    Value = beq[i]
                });
            }

            // Добавляем Box-ограничения
            for (int i = 0; i < n; i++)
            {
                constraints.Add(new LinearConstraint(1) { VariablesAtIndices = new[] { i }, CombinedAs = new[] { 1.0 }, ShouldBe = ConstraintType.GreaterThanOrEqualTo, Value = lb[i] });
                constraints.Add(new LinearConstraint(1) { VariablesAtIndices = new[] { i }, CombinedAs = new[] { 1.0 }, ShouldBe = ConstraintType.LesserThanOrEqualTo, Value = ub[i] });
            }

            Stopwatch sw2 = Stopwatch.StartNew();
            var solver = new GoldfarbIdnani(objective, constraints);
            bool success = solver.Minimize();
            sw2.Stop();

            var reconciledList = vars.Select((v, index) => new BalanceResult
            {
                Id = v.Id,
                Name = v.Name,
                ReconciledValue = success ? (decimal)solver.Solution[index] : v.Measured,
                Measured = v.Measured,
                isMeasured = v.IsMeasured
            }).ToList();

            // Формирование ответа
            return new BalanceResponse
            {
                Status = success ? "FOUND_IN_BOUNDS" : "INFEASIBLE",
                GlobalTestValue = (decimal)gtValue,
                IsGlobalTestPassed = gtValue <= 1.0,
                ReconciledVariables = reconciledList,
                GrossErrorStreams = grossErrorStreams,
                TimeMs = sw2.ElapsedMilliseconds,
                TimeDetectErrorsMs = sw1.ElapsedMilliseconds
            };
        }

        // Поиск грубых ошибок
        private List<string> DetectGrossErrors(double[,] Aeq, List<string> nodes, VariableDto[] vars, double currentGT)
        {
            var errorStreamIds = new List<string>();
            int m = Aeq.GetLength(0);
            int currentN = Aeq.GetLength(1);

            // Предварительное выделение буфера
            int maxExpectedCols = currentN + m * 2;
            double[,] currentAeq = new double[m, maxExpectedCols];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < currentN; j++)
                {
                    currentAeq[i, j] = Aeq[i, j];
                }
            }

            // Пока глобальный тест больше 1, ищем ошибки
            while (currentGT > 1.0)
            {
                double maxDelta = 0.0;
                int bestI = -1;
                int bestJ = -1;

                //// Перебор всех комбинаций
                //for (int i = 0; i < m; i++)
                //{
                //    for (int j = 0; j < i; j++)
                //    {
                //        ApplyShuntToMatrixInPlace(currentAeq, m, currentN, i, j);
                //        try
                //        {
                //            double newGT = CalculateGlobalTest(currentAeq, vars, currentN + 1, m);
                //            double delta = currentGT - newGT;

                //            if (delta > maxDelta)
                //            {
                //                maxDelta = delta;
                //                bestI = i;
                //                bestJ = j;
                //            }
                //        }
                //        catch
                //        {

                //        }
                //        finally
                //        {
                //            ClearShuntFromMatrixInPlace(currentAeq, m, currentN);
                //        }
                //    }
                //}

                // Сборка всех комбинаций (i, j) в список
                var pairs = new List<(int i, int j)>();
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        pairs.Add((i, j));
                    }
                }

                // Безопасное обновление общих переменных в конце
                object lockObj = new object();

                // Сохраняем текущее базовое значение GT
                double baselineGT = currentGT;

                // Распараллеливание
                Parallel.ForEach(
                    pairs,

                    // Создание локального состояния для каждого потока
                    () => new
                    {
                        Matrix = (double[,])currentAeq.Clone(),
                        MaxDelta = double.MinValue,
                        BestI = -1,
                        BestJ = -1,
                    },

                    // Логика для каждой пары (i, j)
                    (pair, loopState, localState) =>
                    {
                        int i = pair.i;
                        int j = pair.j;

                        try
                        {
                            // Работа с локальной копией матрицы
                            ApplyShuntToMatrixInPlace(localState.Matrix, m, currentN, i, j);

                            double newGT = CalculateGlobalTest(localState.Matrix, vars, currentN + 1, m);
                            double delta = baselineGT - newGT;

                            if (delta > localState.MaxDelta)
                            {
                                // Обновление локальных переменных без блокировок
                                return new
                                {
                                    localState.Matrix,
                                    MaxDelta = delta,
                                    BestI = i,
                                    BestJ = j
                                };
                            }
                        }
                        catch
                        {

                        }
                        finally
                        {
                            // Очистка локальной матрицы
                            ClearShuntFromMatrixInPlace(localState.Matrix, m, currentN);
                        }

                        return localState;
                    },

                    // Срабатывает один раз при завершении работы потока
                    // Безопасно мерджится локальный результат потока в общие переменные (через lock)
                    localState =>
                    {
                        if (localState.MaxDelta > double.MinValue)
                        {
                            lock (lockObj)
                            {
                                if (localState.MaxDelta > maxDelta)
                                {
                                    maxDelta = localState.MaxDelta;
                                    bestI = localState.BestI;
                                    bestJ = localState.BestJ;
                                }
                            }
                        }
                    }
                );

                // Если найдено улучшение
                if (bestI != -1 && maxDelta > 0.001)
                {
                    string node1 = bestI < m ? nodes[bestI] : null;
                    string node2 = bestJ < m ? nodes[bestJ] : null;

                    var suspectStream = vars.FirstOrDefault(v =>
                        (v.SourceId == node1 && v.DestinationId == node2) ||
                        (v.SourceId == node2 && v.DestinationId == node1));

                    if (suspectStream != null && !errorStreamIds.Contains(suspectStream.Id))
                    {
                        errorStreamIds.Add(suspectStream.Id);
                    }
                    else
                    {
                        errorStreamIds.Add($"Leak_between_{node1 ?? "Environment"}_and_{node2 ?? "Environment"}");
                    }

                    ApplyShuntToMatrixInPlace(currentAeq, m, currentN, bestI, bestJ);
                    currentN++;
                    currentGT -= maxDelta;
                }
                else
                {
                    break;
                }
            }

            return errorStreamIds;
        }

        private static void ApplyShuntToMatrixInPlace(double[,] matrix, int envIdx, int colIndex, int fromIdx, int toIdx)
        {
            if (fromIdx < envIdx) matrix[fromIdx, colIndex] = -1.0;
            if (toIdx < envIdx) matrix[toIdx, colIndex] = 1.0;
        }

        private static void ClearShuntFromMatrixInPlace(double[,] matrix, int envIdx, int colIndex)
        {
            for (int i = 0; i < envIdx; i++)
            {
                matrix[i, colIndex] = 0.0;
            }
        }

        private double CalculateGlobalTest(double[,] Aeq, VariableDto[] originalVars, int activeCols, int explicitRows = -1)
        {
            int m = explicitRows > 0 ? explicitRows : Aeq.GetLength(0);

            double[] x = new double[activeCols];
            double maxMeasured = 0.0;

            for (int i = 0; i < originalVars.Length && i < activeCols; i++)
            {
                x[i] = (double)originalVars[i].Measured;
                if (x[i] > maxMeasured) maxMeasured = x[i];
            }

            double[,] xSigma = new double[activeCols, activeCols];
            for (int i = 0; i < activeCols; i++)
            {
                if (i < originalVars.Length && originalVars[i].IsMeasured)
                {
                    // CoefDelta - из внедренных настроек
                    double stdDev = (double)originalVars[i].Tolerance / _options.CoefDelta;
                    xSigma[i, i] = stdDev * stdDev;
                }
                else
                {
                    xSigma[i, i] = Math.Pow(10.0 * maxMeasured, 2.0);
                }
            }

            // Извлечение активного фрагмента матрицы
            double[,] A = new double[m, activeCols];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < activeCols; j++)
                    A[i, j] = Aeq[i, j];

            // Расчет вектора разбаланса: r = A * x
            double[] r = A.Dot(x);

            // Расчет ковариационной матрицы разбаланса: V = A * xSigma * A'
            double[,] V = A.Dot(xSigma).Dot(A.Transpose());

            // Псевдоинверсия
            double[,] Vinv = MyPseudoInverse(V);
            // GT_original = r' * inv(V) * r
            double gtOriginal = r.Dot(Vinv).Dot(r);

            // Степени свободы = Ранг матрицы A (в простых схемах равен числу узлов m)
            // Alpha - из внедренных настроек
            var chi2 = new ChiSquareDistribution(m);
            double gtLimit = chi2.InverseDistributionFunction(1.0 - _options.Alpha);

            if (gtOriginal == 0.0 && gtLimit == 0.0) return 0.0;
            return gtOriginal / gtLimit;
        }

        private double[,] MyPseudoInverse(double[,] x, double relativeTolerance = 1e-10)
        {
            int rows = x.GetLength(0);
            int cols = x.GetLength(1);

            var doubleMatrix = DenseMatrix.OfArray(x);

            var svd = doubleMatrix.Svd(true);
            var s = svd.S;
            var u = svd.U;
            var vt = svd.VT;

            double threshold = relativeTolerance * s[0];

            var sInv = DenseMatrix.Create(rows, cols, 0.0);
            for (int i = 0; i < s.Count; i++)
            {
                if (s[i] > threshold)
                    sInv[i, i] = 1.0 / s[i];
            }

            var resDouble = vt.Transpose().Multiply(sInv).Multiply(u.Transpose());

            double[,] result = new double[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    result[i, j] = resDouble[i, j];
                }
            }

            return result;
        }
    }
}
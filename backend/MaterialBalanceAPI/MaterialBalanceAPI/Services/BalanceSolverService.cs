using Accord.Math.Optimization;
using MaterialBalanceAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Distributions.Univariate;
using Accord.Math;
using System;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MaterialBalanceAPI.Services
{
    public interface IBalanceSolverService
    {
        BalanceResponse Solve(BalanceRequest request);
    }
    //public class BalanceSolverService : IBalanceSolverService
    //{
    //    public BalanceResponse Solve(BalanceRequest request)
    //    {
    //        var vars = request.Variables;
    //        int n = vars.Count;

    //        // 1. Формируем список уникальных узлов
    //        var nodes = vars.Select(v => v.SourceId)
    //                        .Concat(vars.Select(v => v.DestinationId))
    //                        .Where(id => id != null)
    //                        .Distinct()
    //                        .ToList();

    //        int m = nodes.Count;

    //        // 2. Строим матрицу Aeq (балансы) и beq
    //        // Для каждого узла сумма входящих равна сумме исходящих
    //        decimal[,] Aeq = new decimal[m, n];
    //        decimal[] beq = new decimal[m]; // По умолчанию нули

    //        for (int i = 0; i < m; i++)
    //        {
    //            string nodeId = nodes[i];
    //            for (int j = 0; j < n; j++)
    //            {
    //                if (vars[j].DestinationId == nodeId) Aeq[i, j] = 1.0m;  // Входит в узел
    //                else if (vars[j].SourceId == nodeId) Aeq[i, j] = -1.0m; // Выходит из узла
    //                else Aeq[i, j] = 0.0m;
    //            }
    //        }

    //        decimal GTValue = CalculateGlobalTest(Aeq, vars.ToArray()); // Расчет глобального теста до балансировки
    //        decimal GTValue_copy = GTValue;

    //        List<string> grossErrorStreams = DetectGrossErrors(Aeq, nodes, vars.ToArray(), GTValue_copy);

    //        // 3. Формируем H (веса) и f (линейный член)
    //        decimal[,] H = new decimal[n, n];
    //        decimal[] f = new decimal[n];

    //        for (int i = 0; i < n; i++)
    //        {
    //            if (vars[i].IsMeasured && vars[i].Tolerance > 0.0m)
    //            {
    //                // Вес обычно обратно пропорционален квадрату допуска
    //                decimal weight = 1.0m / (vars[i].Tolerance * vars[i].Tolerance);
    //                H[i, i] = weight;
    //                f[i] = -weight * vars[i].Measured;
    //            }
    //            else
    //            {
    //                H[i, i] = 1e-6m; // Малый вес для неизмеряемых/вычисляемых параметров для устойчивости матрицы
    //                f[i] = 0.0m;
    //            }
    //        }

    //        // 4. Ограничения lb (нижние) и ub (верхние)
    //        decimal[] lb = vars.Select(v => v.MinBound).ToArray();
    //        decimal[] ub = vars.Select(v => v.MaxBound).ToArray();

    //        double[,] H_double = new double[n, n];
    //        double[] f_double = new double[n];
    //        for (int i = 0; i < n; i++)
    //        {
    //            for (int j = 0; j < n; j++)
    //            {
    //                H_double[i, j] = (double)H[i, j];
    //            }
    //            f_double[i] = (double)f[i];
    //        }

    //        // 5. Инициализация солвера (алгоритм Goldfarb-Idnani)
    //        var objective = new QuadraticObjectiveFunction(H_double, f_double);
    //        var constraints = new List<LinearConstraint>();

    //        // Добавляем балансовые ограничения (Aeq * x = beq)
    //        for (int i = 0; i < m; i++)
    //        {
    //            decimal[] row = new decimal[n];
    //            double[] row_double = new double[n];
    //            for (int j = 0; j < n; j++)
    //            {
    //                row[j] = Aeq[i, j];
    //                row_double[j] = (double)row[j];
    //            }
    //            constraints.Add(new LinearConstraint(n) { VariablesAtIndices = Enumerable.Range(0, n).ToArray(), CombinedAs = row_double, ShouldBe = ConstraintType.EqualTo, Value = (double)beq[i] });
    //        }

    //        // Добавляем Box-ограничения
    //        for (int i = 0; i < n; i++)
    //        {
    //            constraints.Add(new LinearConstraint(1) { VariablesAtIndices = new[] { i }, CombinedAs = new[] { 1.0 }, ShouldBe = ConstraintType.GreaterThanOrEqualTo, Value = (double)lb[i] });
    //            constraints.Add(new LinearConstraint(1) { VariablesAtIndices = new[] { i }, CombinedAs = new[] { 1.0 }, ShouldBe = ConstraintType.LesserThanOrEqualTo, Value = (double)ub[i] });
    //        }

    //        var solver = new GoldfarbIdnani(objective, constraints);
    //        bool success = solver.Minimize();

    //        var reconciledList = vars.Select((v, index) => new BalanceResult
    //        {
    //            Id = v.Id,
    //            Name = v.Name,
    //            ReconciledValue = success ? (decimal)solver.Solution[index] : v.Measured,
    //            Measured = v.Measured,
    //            isMeasured = v.IsMeasured
    //        }).ToList();

    //        // Формирование ответа
    //        var response = new BalanceResponse
    //        {
    //            Status = success ? "FOUND_IN_BOUNDS" : "INFEASIBLE",
    //            GlobalTestValue = GTValue,
    //            IsGlobalTestPassed = GTValue <= 1.0m, // Порог для прохождения глобального теста
    //            ReconciledVariables = reconciledList,
    //            GrossErrorStreams = grossErrorStreams
    //        };

    //        return response;
    //    }

    //    // Поиск грубых ошибок
    //    private List<string> DetectGrossErrors(decimal[,] Aeq, List<string> nodes, VariableDto[] vars, decimal currentGT)
    //    {
    //        var errorStreamIds = new List<string>();
    //        int m = Aeq.GetLength(0);
    //        decimal[,] currentAeq = (decimal[,])Aeq.Clone();
    //        int currentN = Aeq.GetLength(1);

    //        // Пока глобальный тест больше 1, ищем ошибки
    //        while (currentGT > 1.0m)
    //        {
    //            decimal maxDelta = 0.0m;
    //            int bestI = -1;
    //            int bestJ = -1;

    //            // Перебор всех комбинаций
    //            for (int i = 0; i < m; i++)
    //            {
    //                for (int j = 0; j < i; j++)
    //                {
    //                    var testAeq = AddShuntToMatrix(currentAeq, i, j, m);
    //                    try
    //                    {
    //                        decimal newGT = CalculateGlobalTestForShunts(testAeq, vars, currentN);

    //                        decimal delta = currentGT - newGT;

    //                        if (delta > maxDelta)
    //                        {
    //                            maxDelta = delta;
    //                            bestI = i;
    //                            bestJ = j;
    //                        }
    //                    }
    //                    catch
    //                    {

    //                    }
    //                }
    //            }

    //            // Если найдено улучшение
    //            if (bestI != -1 && maxDelta > 0.001m)
    //            {
    //                string node1 = bestI < m ? nodes[bestI] : null;
    //                string node2 = bestJ < m ? nodes[bestJ] : null;

    //                var suspectStream = vars.FirstOrDefault(v =>
    //                (v.SourceId == node1 && v.DestinationId == node2) ||
    //                (v.SourceId == node2 && v.DestinationId == node1));

    //                if (suspectStream != null && !errorStreamIds.Contains(suspectStream.Id))
    //                {
    //                    errorStreamIds.Add(suspectStream.Id);
    //                }
    //                else
    //                {
    //                    errorStreamIds.Add($"Leak_between_{node1 ?? "Environment"}_and_{node2 ?? "Environment"}");
    //                }

    //                currentAeq = AddShuntToMatrix(currentAeq, bestI, bestJ, m);
    //                currentN++;
    //                currentGT -= maxDelta;
    //            }
    //            else
    //            {
    //                break;
    //            }
    //        }
    //        return errorStreamIds;
    //    }

    //    private decimal[,] AddShuntToMatrix(decimal[,] A, int fromIdx, int toIdx, int envIdx)
    //    {
    //        int rows = A.GetLength(0);
    //        int cols = A.GetLength(1);
    //        decimal[,] newA = new decimal[rows, cols + 1];

    //        for (int i = 0; i < rows; i++)
    //            for (int j = 0; j < cols; j++)
    //                newA[i, j] = A[i, j];

    //        if (fromIdx < envIdx) newA[fromIdx, cols] = -1.0m;
    //        if (toIdx < envIdx) newA[toIdx, cols] = 1.0m;

    //        return newA;
    //    }

    //    private decimal CalculateGlobalTestForShunts(decimal[,] Aeq, VariableDto[] originalVars, int originalN)
    //    {
    //        int m = Aeq.GetLength(0);
    //        int n_total = Aeq.GetLength(1);

    //        decimal[] _x = new decimal[n_total];
    //        for (int i = 0; i < originalN; i++) _x[i] = originalVars[i].Measured;

    //        decimal maxMeasured = _x.Length > 0.0m ? _x.Max() : 0.0m; // Для нормировки, если есть измеренные значения
    //        decimal coef_delta = 1.96m;

    //        decimal[,] xSigma = new decimal[n_total, n_total];
    //        for (int i = 0; i < n_total; i++)
    //        {
    //            if (i < originalN && originalVars[i].IsMeasured)
    //            {
    //                decimal stdDev = originalVars[i].Tolerance / coef_delta; // Стандартное отклонение, предполагая 95% доверительный интервал
    //                xSigma[i, i] = stdDev * stdDev; // Дисперсия
    //            }
    //            else
    //            {
    //                // Для неизмеряемых потоков - очень большая погрешность
    //                xSigma[i, i] = (decimal)Math.Pow((double)(10.0m * maxMeasured), (double)2.0m);
    //            }
    //        }

    //        // Расчет вектора разбаланса r = A * _x
    //        decimal[] r = Aeq.Dot(_x);

    //        // Расчет ковариационной матрицы разбаланса V = Aeq * xSigma * Aeq'
    //        decimal[,] V = Aeq.Dot(xSigma).Dot(Aeq.Transpose());

    //        // GT_original = r' * inv(V) * r
    //        decimal[,] Vinv = MyPseudoInverse(V);
    //        decimal GTOriginal = r.Dot(Vinv).Dot(r);

    //        // Расчет порога (Квантиль Хи-квадрат)
    //        decimal alpha = 0.05m;
    //        // Степени свободы = Ранг матрицы A (в простых схемах равен числу узлов m)
    //        var chi2 = new ChiSquareDistribution(m);
    //        decimal GTLimit = (decimal)chi2.InverseDistributionFunction((double)(1.0m - alpha));

    //        // Нормируем GT на порог
    //        if (GTOriginal == 0.0m && GTLimit == 0.0m) return 0.0m;
    //        return GTOriginal / GTLimit;
    //    }

    //    private decimal CalculateGlobalTest(decimal[,] Aeq, VariableDto[] vars)
    //    {
    //        int m = Aeq.GetLength(0); // Число узлов
    //        int n = Aeq.GetLength(1); // Число потоков

    //        decimal[] _x = vars.Select(v => v.Measured).ToArray(); // Вектор измеренных значений
    //        decimal maxMeasured = _x.Length > 0.0m ? _x.Max() : 0.0m; // Для нормировки, если есть измеренные значения
    //        decimal coef_delta = 1.96m;

    //        decimal[] variances = new decimal[n];
    //        for (int i = 0; i < n; i++)
    //        {
    //            if (vars[i].IsMeasured)
    //            {
    //                decimal stdDev = vars[i].Tolerance / coef_delta; // Стандартное отклонение, предполагая 95% доверительный интервал
    //                variances[i] = stdDev * stdDev; // Дисперсия
    //            }
    //            else
    //            {
    //                // Для неизмеряемых потоков - очень большая погрешность
    //                variances[i] = (decimal)Math.Pow((double)(10.0m * maxMeasured), (double)2.0m);
    //            }
    //        }
    //        //decimal[,] xSigma = Matrix.Diagonal(variances); // Диагональная матрица дисперсий
    //        decimal[,] xSigma = new decimal[n, n];
    //        for (int i = 0; i < n; i++)
    //        {
    //            xSigma[i, i] = variances[i];
    //        }

    //        // Расчет вектора разбаланса r = A * _x
    //        decimal[] r = Aeq.Dot(_x);

    //        // Расчет ковариационной матрицы разбаланса V = Aeq * xSigma * Aeq'
    //        decimal[,] V = Aeq.Dot(xSigma).Dot(Aeq.Transpose());

    //        // GT_original = r' * inv(V) * r
    //        decimal[,] Vinv = MyPseudoInverse(V);
    //        decimal GTOriginal = r.Dot(Vinv).Dot(r);

    //        // Расчет порога (Квантиль Хи-квадрат)
    //        decimal alpha = 0.05m;
    //        // Степени свободы = Ранг матрицы A (в простых схемах равен числу узлов m)
    //        var chi2 = new ChiSquareDistribution(m);
    //        decimal GTLimit = (decimal)chi2.InverseDistributionFunction((double)(1.0m - alpha));

    //        // Нормируем GT на порог
    //        if (GTOriginal == 0.0m && GTLimit == 0.0m) return 0.0m;
    //        return GTOriginal / GTLimit;
    //    }

    //    private decimal[,] MyPseudoInverse(decimal[,] x, double relativeTolerance = 1e-10)
    //    {
    //        int rows = x.GetLength(0);
    //        int cols = x.GetLength(1);

    //        var doubleMatrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Build.Dense(rows, cols);
    //        for (int i = 0; i < rows; i++)
    //        {
    //            for (int j = 0; j < cols; j++)
    //            {
    //                doubleMatrix[i, j] = (double)x[i, j];
    //            }
    //        }

    //        var svd = doubleMatrix.Svd(true);
    //        var s = svd.S;
    //        var u = svd.U;
    //        var vt = svd.VT;

    //        double threshold = relativeTolerance * s[0];

    //        var sInv = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.Build.Dense(rows, cols);
    //        for (int i = 0; i < s.Count; i++)
    //        {
    //            if (s[i] > threshold)
    //                sInv[i, i] = 1.0 / s[i];
    //            else
    //                sInv[i, i] = 0.0;
    //        }

    //        var resDouble = vt.Transpose().Multiply(sInv).Multiply(u.Transpose());

    //        decimal[,] result = new decimal[cols, rows];
    //        for (int i = 0; i < cols; i++)
    //        {
    //            for (int j = 0; j < rows; j++)
    //            {
    //                result[i, j] = (decimal)resDouble[i, j];
    //            }
    //        }

    //        return result;
    //    }
    //}

    public class BalanceSolverService : IBalanceSolverService
    {
        private const double CoefDelta = 1.96;
        private const double Alpha = 0.05;

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
            var grossErrorStreams = DetectGrossErrors(Aeq, nodes, varsArray, gtValue);

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

            var solver = new GoldfarbIdnani(objective, constraints);
            bool success = solver.Minimize();

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
                GrossErrorStreams = grossErrorStreams
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

                // Перебор всех комбинаций
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        ApplyShuntToMatrixInPlace(currentAeq, m, currentN, i, j);
                        try
                        {
                            double newGT = CalculateGlobalTest(currentAeq, vars, currentN + 1, m);
                            double delta = currentGT - newGT;

                            if (delta > maxDelta)
                            {
                                maxDelta = delta;
                                bestI = i;
                                bestJ = j;
                            }
                        }
                        catch
                        {
                            
                        }
                        finally
                        {
                            ClearShuntFromMatrixInPlace(currentAeq, m, currentN);
                        }
                    }
                }

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
                    double stdDev = (double)originalVars[i].Tolerance / CoefDelta;
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
            var chi2 = new ChiSquareDistribution(m);
            double gtLimit = chi2.InverseDistributionFunction(1.0 - Alpha);

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
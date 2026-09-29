using Accord.Math;
using MaterialBalanceAPI.Models;
using MaterialBalanceAPI.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var solver = new BalanceSolverService();
            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>
                {
                    new VariableDto
                    {
                        Id = "1",
                        Name = "In1",
                        SourceId = null,
                        DestinationId = "Node1",
                        Measured = 10.0054919341489m,
                        Tolerance = 2.0m * 10.0054919341489m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "2",
                        Name = "Out1",
                        SourceId = "Node1",
                        DestinationId = null,
                        Measured = 3.03265795024749m,
                        Tolerance = 4.0m * 3.03265795024749m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "3",
                        Name = "Out1In2",
                        SourceId = "Node1",
                        DestinationId = "Node2",
                        Measured = 6.83122010827837m,
                        Tolerance = 10.0m * 6.83122010827837m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "4",
                        Name = "Out2",
                        SourceId = "Node2",
                        DestinationId = null,
                        Measured = 1.98478460320379m,
                        Tolerance = 2.0m * 1.98478460320379m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "5",
                        Name = "Out2In3",
                        SourceId = "Node2",
                        DestinationId = "Node3",
                        Measured = 5.09293357450987m,
                        Tolerance = 2.0m * 5.09293357450987m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "6",
                        Name = "Out3",
                        SourceId = "Node3",
                        DestinationId = null,
                        Measured = 4.05721328676762m,
                        Tolerance = 2.0m * 4.05721328676762m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "7",
                        Name = "Out3",
                        SourceId = "Node3",
                        DestinationId = null,
                        Measured = 0.991215230484718m,
                        Tolerance = 2.0m * 0.991215230484718m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    }
                }
            };

            var result = solver.Solve(request);
            Assert.AreEqual("FOUND_IN_BOUNDS", result.Status);

            // Ответы
            double[] answers = { 10.0555820555568, 3.0142509913592, 7.04133106419761, 1.98210405567851,
                                 5.0592270085191, 4.06740355102557, 0.991823457493527 };
            for (int i = 0; i < result.ReconciledVariables.Count; i++)
            {
                double diff = Math.Abs((double)result.ReconciledVariables[i].ReconciledValue - answers[i]);
                Assert.True(diff < 1e-13, $"Поток x{i + 1}: ожидаемое значение {answers[i]}, полученное значение {result.ReconciledVariables[i].ReconciledValue}, разница {diff}");
            }
        }

        [Test]
        public void Test2()
        {
            var solver = new BalanceSolverService();
            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>
                {
                    new VariableDto
                    {
                        Id = "1",
                        Name = "In1",
                        SourceId = null,
                        DestinationId = "Node1",
                        Measured = 10.0054919341489m,
                        Tolerance = 2.0m * 10.0054919341489m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "2",
                        Name = "Out1",
                        SourceId = "Node1",
                        DestinationId = null,
                        Measured = 3.03265795024749m,
                        Tolerance = 4.0m * 3.03265795024749m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "3",
                        Name = "Out1In2",
                        SourceId = "Node1",
                        DestinationId = "Node2",
                        Measured = 6.83122010827837m,
                        Tolerance = 10.0m * 6.83122010827837m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "4",
                        Name = "Out2",
                        SourceId = "Node2",
                        DestinationId = null,
                        Measured = 1.98478460320379m,
                        Tolerance = 2.0m * 1.98478460320379m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "5",
                        Name = "Out2In3",
                        SourceId = "Node2",
                        DestinationId = "Node3",
                        Measured = 5.09293357450987m,
                        Tolerance = 2.0m * 5.09293357450987m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "6",
                        Name = "Out3",
                        SourceId = "Node3",
                        DestinationId = null,
                        Measured = 4.05721328676762m,
                        Tolerance = 2.0m * 4.05721328676762m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "7",
                        Name = "Out3",
                        SourceId = "Node3",
                        DestinationId = null,
                        Measured = 0.991215230484718m,
                        Tolerance = 2.0m * 0.991215230484718m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    },
                    new VariableDto
                    {
                        Id = "8",
                        Name = "Out1",
                        SourceId = "Node1",
                        DestinationId = null,
                        Measured = 6.66666m,
                        Tolerance = 10.0m * 6.66666m / 100.0m,
                        IsMeasured = true,
                        MinBound = 0,
                        MaxBound = 1000
                    }
                }
            };

            var result = solver.Solve(request);
            Assert.AreEqual("FOUND_IN_BOUNDS", result.Status);

            // Ответы
            double[] answers = { 10.5402456913604, 2.83614833622227, 6.97261297632469, 1.9632643552402,
                                 5.0093486210845, 4.02033457294678, 0.989014048137712, 0.731484378813389 };
            for (int i = 0; i < result.ReconciledVariables.Count; i++)
            {
                double diff = Math.Abs((double)result.ReconciledVariables[i].ReconciledValue - answers[i]);
                Assert.True(diff < 1e-13, $"Поток x{i + 1}: ожидаемое значение {answers[i]}, полученное значение {result.ReconciledVariables[i].ReconciledValue}, разница {diff}");
            }
        }

        [Test]
        public void Solve_SimpleSplitter_ConservesMass()
        {
            // Arrange
            var solver = new BalanceSolverService();
            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>
            {
                // Входной поток (100) делится на два (40 и 60)
                new VariableDto { Id = "1", Name = "In", DestinationId = "Node1", Measured = 101, Tolerance = 2, IsMeasured = true, MinBound = 0, MaxBound = 1000 },
                new VariableDto { Id = "2", Name = "Out1", SourceId = "Node1", Measured = 38, Tolerance = 1, IsMeasured = true, MinBound = 0, MaxBound = 1000 },
                new VariableDto { Id = "3", Name = "Out2", SourceId = "Node1", Measured = 59, Tolerance = 1, IsMeasured = true, MinBound = 0, MaxBound = 1000 }
            }
            };

            // Act
            var result = solver.Solve(request);

            // Assert
            Assert.AreEqual("FOUND_IN_BOUNDS", result.Status);

            double input = (double)result.ReconciledVariables.First(v => v.Id == "1").ReconciledValue;
            double out1 = (double)result.ReconciledVariables.First(v => v.Id == "2").ReconciledValue;
            double out2 = (double)result.ReconciledVariables.First(v => v.Id == "3").ReconciledValue;

            // Проверяем сведение баланса: Вход = Выход1 + Выход2 (с учетом погрешности double)
            Assert.True(System.Math.Abs(input - (out1 + out2)) < 1e-4);
        }
    }
}

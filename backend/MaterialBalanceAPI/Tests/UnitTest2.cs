using Accord.Math;
using MaterialBalanceAPI.Models;
using MaterialBalanceAPI.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tests2
    {
        [Test]
        public void TestSolverBalance()
        {
            var solver = new BalanceSolverService();
            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>
            {
                new VariableDto
  {
    Id= "E01",
    Name= "Руда исходная",
    SourceId= null,
    DestinationId= "N01",
    Measured= 1000.0m,
    Tolerance= 20.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E02",
    Name= "Питание флотации",
    SourceId= "N01",
    DestinationId= "N02",
    Measured= 980.5m,
    Tolerance= 19.61m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E03",
    Name= "Концентрат основной флотации",
    SourceId= "N02",
    DestinationId= "N03",
    Measured= 120.3m,
    Tolerance= 2.406m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E04",
    Name= "Хвосты основной флотации",
    SourceId= "N02",
    DestinationId= "N07",
    Measured= 860.2m,
    Tolerance= 17.204m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E05",
    Name= "Концентрат перечистки",
    SourceId= "N03",
    DestinationId= "N04",
    Measured= 98.7m,
    Tolerance= 1.974m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E06",
    Name= "Хвосты перечистки",
    SourceId= "N03",
    DestinationId= "N02",
    Measured= 21.6m,
    Tolerance= 0.432m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E07",
    Name= "Сгущённый продукт",
    SourceId= "N04",
    DestinationId= "N05",
    Measured= 95.4m,
    Tolerance= 1.908m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E08",
    Name= "Слив сгустителя",
    SourceId= "N04",
    DestinationId= "N07",
    Measured= 3.3m,
    Tolerance= 0.066m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E09",
    Name= "Кек фильтра",
    SourceId= "N05",
    DestinationId= "N06",
    Measured= 92.1m,
    Tolerance= 1.842m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E10",
    Name= "Фильтрат",
    SourceId= "N05",
    DestinationId= "N02",
    Measured= 3.3m,
    Tolerance= 0.066m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E11",
    Name= "Концентрат товарный",
    SourceId= "N06",
    DestinationId= null,
    Measured= 92.1m,
    Tolerance= 1.842m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E12",
    Name= "Хвосты товарные",
    SourceId= "N07",
    DestinationId= null,
    Measured= 863.5m,
    Tolerance= 17.27m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E13",
    Name= "Вода в мельницу",
    SourceId= null,
    DestinationId= "N01",
    Measured= 300.0m,
    Tolerance= 6.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E14",
    Name= "Вода в основную флотацию",
    SourceId= null,
    DestinationId= "N02",
    Measured= 150.0m,
    Tolerance= 3.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E15",
    Name= "Реагент-собиратель",
    SourceId= null,
    DestinationId= "N02",
    Measured= 1.2m,
    Tolerance= 0.024m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  },
                new VariableDto
  {
    Id= "E16",
    Name= "Воздух флотации",
    SourceId= null,
    DestinationId= "N02",
    Measured= 50.0m,
    Tolerance= 1.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 10000.0m
  }
            }
            };

            var result = solver.Solve(request);
            Assert.AreEqual("FOUND_IN_BOUNDS", result.Status);

            // Ответы
            double[] answers = { 650.518499228888, 919.065164159488, 121.076665860421, 1013.7258193044,
                                 99.5017044846859, 21.5749613757351, 96.2009256285165, 3.30077885616936,
                                 92.9033646105421, 3.2975610179744, 92.9033646105421, 1017.02659816057,
                                 268.5466649306, 140.698854496439, 1.19940472668777, 48.9665393884932 };
            List<string> grossErrorStreams = new List<string> { "E04", "Leak_between_N06_and_N01" };
            for (int i = 0; i < result.ReconciledVariables.Count; i++)
            {
                double diff = Math.Abs((double)result.ReconciledVariables[i].ReconciledValue - answers[i]);
                Assert.True(diff < 1e-13, $"Поток x{i + 1}= ожидаемое значение {answers[i]}, полученное значение {result.ReconciledVariables[i].ReconciledValue}, разница {diff}");
            }

            Assert.AreEqual(grossErrorStreams, result.GrossErrorStreams);
        }

        [Test]
        public void TestSolverBalance2()
        {
            var solver = new BalanceSolverService();
            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>
            {
                    new VariableDto
  {
    Id= "E01",
    Name= "Нефть сырая",
    SourceId= null,
    DestinationId= "N01",
    Measured= 15000.0m,
    Tolerance= 300.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E02",
    Name= "Обессоленная нефть",
    SourceId= "N01",
    DestinationId= "N02",
    Measured= 14980.0m,
    Tolerance= 299.6m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E03",
    Name= "Нагретая нефть",
    SourceId= "N02",
    DestinationId= "N03",
    Measured= 14980.0m,
    Tolerance= 299.6m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E04",
    Name= "Углеводородный газ",
    SourceId= "N03",
    DestinationId= "N05",
    Measured= 450.0m,
    Tolerance= 9.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E05",
    Name= "Бензиновая фракция",
    SourceId= "N03",
    DestinationId= "N06",
    Measured= 1800.0m,
    Tolerance= 36.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E06",
    Name= "Керосиновая фракция",
    SourceId= "N03",
    DestinationId= "N07",
    Measured= 1200.0m,
    Tolerance= 24.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E07",
    Name= "Дизельная фракция",
    SourceId= "N03",
    DestinationId= "N08",
    Measured= 3500.0m,
    Tolerance= 70.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E08",
    Name= "Мазут",
    SourceId= "N03",
    DestinationId= "N09",
    Measured= 8030.0m,
    Tolerance= 160.6m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E09",
    Name= "Мазут в вакуумную колонну",
    SourceId= "N09",
    DestinationId= "N04",
    Measured= 8030.0m,
    Tolerance= 160.6m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E10",
    Name= "Вакуумный газойль",
    SourceId= "N04",
    DestinationId= "N08",
    Measured= 2500.0m,
    Tolerance= 50.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E11",
    Name= "Гудрон",
    SourceId= "N04",
    DestinationId= "N10",
    Measured= 5530.0m,
    Tolerance= 110.6m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E12",
    Name= "Газ товарный",
    SourceId= "N05",
    DestinationId= null,
    Measured= 450.0m,
    Tolerance= 9.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E13",
    Name= "Бензин товарный",
    SourceId= "N06",
    DestinationId= null,
    Measured= 1800.0m,
    Tolerance= 36.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E14",
    Name= "Керосин товарный",
    SourceId= "N07",
    DestinationId= null,
    Measured= 1200.0m,
    Tolerance= 24.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E15",
    Name= "Дизель товарный",
    SourceId= "N08",
    DestinationId= null,
    Measured= 6000.0m,
    Tolerance= 120.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E16",
    Name= "Гудрон товарный",
    SourceId= "N10",
    DestinationId= null,
    Measured= 5530.0m,
    Tolerance= 110.6m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
        new VariableDto
  {
    Id= "E17",
    Name= "Пар в печь",
    SourceId= null,
    DestinationId= "N02",
    Measured= 200.0m,
    Tolerance= 4.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  }
            }
            };

            var result = solver.Solve(request);
            Assert.AreEqual("FOUND_IN_BOUNDS", result.Status);

            // Ответы
            double[] answers = { 14812.7692308962, 14812.7692308962, 15012.706136073, 450.144951683954,
                                 1802.31922694327, 1201.03076753034, 3512.32030240324, 8046.89088751225,
                                 8046.89088751225, 2503.01147113286, 5543.87941637938, 450.144951683954,
                                 1802.31922694327, 1201.03076753034, 6015.3317735361, 5543.87941637938,
                                 199.936905176816 };
            List<string> grossErrorStreams = new List<string> { };
            for (int i = 0; i < result.ReconciledVariables.Count; i++)
            {
                double diff = Math.Abs((double)result.ReconciledVariables[i].ReconciledValue - answers[i]);
                Assert.True(diff < 1e-13, $"Поток x{i + 1}= ожидаемое значение {answers[i]}, полученное значение {result.ReconciledVariables[i].ReconciledValue}, разница {diff}");
            }

            Assert.AreEqual(grossErrorStreams, result.GrossErrorStreams);
        }

        [Test]
        public void TestSolverBalance3()
        {
            var solver = new BalanceSolverService();
            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>
            {
                    new VariableDto
  {
    Id= "E01",
    Name= "Железная руда",
    SourceId= null,
    DestinationId= "N01",
    Measured= 30000.0m,
    Tolerance= 600.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E02",
    Name= "Кокс",
    SourceId= null,
    DestinationId= "N01",
    Measured= 8000.0m,
    Tolerance= 160.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E03",
    Name= "Флюс (известняк)",
    SourceId= null,
    DestinationId= "N01",
    Measured= 4000.0m,
    Tolerance= 80.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E04",
    Name= "Шихта в печь",
    SourceId= "N01",
    DestinationId= "N02",
    Measured= 42000.0m,
    Tolerance= 840.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E05",
    Name= "Дутьё (воздух)",
    SourceId= null,
    DestinationId= "N02",
    Measured= 10000.0m,
    Tolerance= 200.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E06",
    Name= "Чугун жидкий",
    SourceId= "N02",
    DestinationId= "N03",
    Measured= 25000.0m,
    Tolerance= 500.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E07",
    Name= "Доменный газ",
    SourceId= "N02",
    DestinationId= "N05",
    Measured= 12000.0m,
    Tolerance= 240.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E08",
    Name= "Шлак доменный",
    SourceId= "N02",
    DestinationId= "N08",
    Measured= 5000.0m,
    Tolerance= 100.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E09",
    Name= "Чугун в конвертер",
    SourceId= "N03",
    DestinationId= "N04",
    Measured= 25000.0m,
    Tolerance= 500.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E10",
    Name= "Скрап",
    SourceId= null,
    DestinationId= "N04",
    Measured= 5000.0m,
    Tolerance= 100.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E11",
    Name= "Кислород",
    SourceId= null,
    DestinationId= "N04",
    Measured= 3000.0m,
    Tolerance= 60.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E12",
    Name= "Сталь жидкая",
    SourceId= "N04",
    DestinationId= "N05",
    Measured= 28000.0m,
    Tolerance= 560.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E13",
    Name= "Конвертерный газ",
    SourceId= "N04",
    DestinationId= "N10",
    Measured= 2000.0m,
    Tolerance= 40.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E14",
    Name= "Шлак конвертерный",
    SourceId= "N04",
    DestinationId= "N08",
    Measured= 3000.0m,
    Tolerance= 60.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E15",
    Name= "Сталь в МНЛЗ",
    SourceId= "N05",
    DestinationId= "N06",
    Measured= 27500.0m,
    Tolerance= 550.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E16",
    Name= "Слябы",
    SourceId= "N06",
    DestinationId= "N07",
    Measured= 27000.0m,
    Tolerance= 540.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E17",
    Name= "Обрезь/облой",
    SourceId= "N06",
    DestinationId= "N09",
    Measured= 500.0m,
    Tolerance= 10.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E18",
    Name= "Скрап оборотный",
    SourceId= "N09",
    DestinationId= "N04",
    Measured= 500.0m,
    Tolerance= 10.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  }
            }
            };

            var result = solver.Solve(request);
            Assert.AreEqual("INFEASIBLE", result.Status);

            // Ответы
            double[] answers = { 30000, 8000, 4000, 42000, 10000, 25000, 12000, 5000, 25000, 5000, 3000, 28000, 2000, 3000, 27500,
                                 27000, 500, 500 };
            List<string> grossErrorStreams = new List<string> { "Leak_between_N07_and_N01", "Leak_between_N08_and_N01",
                "Leak_between_N10_and_N01", "Leak_between_N05_and_N01", "E04", "Leak_between_N07_and_N04" };
            for (int i = 0; i < result.ReconciledVariables.Count; i++)
            {
                double diff = Math.Abs((double)result.ReconciledVariables[i].ReconciledValue - answers[i]);
                Assert.True(diff < 1e-13, $"Поток x{i + 1}= ожидаемое значение {answers[i]}, полученное значение {result.ReconciledVariables[i].ReconciledValue}, разница {diff}");
            }

            Assert.AreEqual(grossErrorStreams, result.GrossErrorStreams);
        }
    }
}

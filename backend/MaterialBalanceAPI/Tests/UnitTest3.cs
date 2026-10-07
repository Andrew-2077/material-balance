using Accord.Math;
using MaterialBalanceAPI.Models;
using MaterialBalanceAPI.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class Tests3
    {
        [Test]
        public void TestSolverBalance60()
        {
            var solver = new BalanceSolverService();
            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>
            {
                    new VariableDto
  {
    Id= "E01",
    Name= "Уголь на склад",
    SourceId= null,
    DestinationId= "N01",
    Measured= 12000.0m,
    Tolerance= 240.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E02",
    Name= "Уголь в коксовую батарею",
    SourceId= "N01",
    DestinationId= "N02",
    Measured= 11900.0m,
    Tolerance= 238.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E03",
    Name= "Потери угля при хранении",
    SourceId= "N01",
    DestinationId= null,
    Measured= 100.0m,
    Tolerance= 2.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E04",
    Name= "Кокс сырой",
    SourceId= "N02",
    DestinationId= "N03",
    Measured= 8500.0m,
    Tolerance= 170.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E05",
    Name= "Коксовый газ",
    SourceId= "N02",
    DestinationId= "N18",
    Measured= 1900.0m,
    Tolerance= 38.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E06",
    Name= "Смола и хим. продукты",
    SourceId= "N02",
    DestinationId= null,
    Measured= 1500.0m,
    Tolerance= 30.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E07",
    Name= "Кокс товарный (в домну)",
    SourceId= "N03",
    DestinationId= "N07",
    Measured= 8200.0m,
    Tolerance= 164.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E08",
    Name= "Коксовая мелочь",
    SourceId= "N03",
    DestinationId= "N06",
    Measured= 250.0m,
    Tolerance= 5.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E09",
    Name= "Отсев кокса (потери)",
    SourceId= "N03",
    DestinationId= null,
    Measured= 50.0m,
    Tolerance= 1.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E10",
    Name= "Руда на склад",
    SourceId= null,
    DestinationId= "N04",
    Measured= 40000.0m,
    Tolerance= 800.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E11",
    Name= "Руда в агломерацию",
    SourceId= "N04",
    DestinationId= "N06",
    Measured= 39500.0m,
    Tolerance= 790.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E12",
    Name= "Потери руды при хранении",
    SourceId= "N04",
    DestinationId= null,
    Measured= 500.0m,
    Tolerance= 10.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E13",
    Name= "Флюсы на склад",
    SourceId= null,
    DestinationId= "N05",
    Measured= 8000.0m,
    Tolerance= 160.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E14",
    Name= "Флюсы в агломерацию",
    SourceId= "N05",
    DestinationId= "N06",
    Measured= 7800.0m,
    Tolerance= 156.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E15",
    Name= "Потери флюсов",
    SourceId= "N05",
    DestinationId= null,
    Measured= 200.0m,
    Tolerance= 4.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E16",
    Name= "Агломерат",
    SourceId= "N06",
    DestinationId= "N07",
    Measured= 46500.0m,
    Tolerance= 930.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E17",
    Name= "Отходящие газы агломерации",
    SourceId= "N06",
    DestinationId= "N17",
    Measured= 900.0m,
    Tolerance= 18.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E18",
    Name= "Возврат агломерата",
    SourceId= "N06",
    DestinationId= "N19",
    Measured= 150.0m,
    Tolerance= 3.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E19",
    Name= "Кокс в домну",
    SourceId= "N03",
    DestinationId= "N07",
    Measured= 8200.0m,
    Tolerance= 164.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E20",
    Name= "Агломерат в домну",
    SourceId= "N06",
    DestinationId= "N07",
    Measured= 46500.0m,
    Tolerance= 930.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E21",
    Name= "Кислород в домну",
    SourceId= "N08",
    DestinationId= "N07",
    Measured= 3000.0m,
    Tolerance= 60.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E22",
    Name= "Природный газ в домну",
    SourceId= null,
    DestinationId= "N07",
    Measured= 2000.0m,
    Tolerance= 40.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E23",
    Name= "Чугун жидкий",
    SourceId= "N07",
    DestinationId= "N09",
    Measured= 30000.0m,
    Tolerance= 600.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E24",
    Name= "Чугун в конвертер №2",
    SourceId= "N07",
    DestinationId= "N10",
    Measured= 25000.0m,
    Tolerance= 500.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E25",
    Name= "Доменный газ",
    SourceId= "N07",
    DestinationId= "N18",
    Measured= 18000.0m,
    Tolerance= 360.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E26",
    Name= "Шлак доменный",
    SourceId= "N07",
    DestinationId= "N16",
    Measured= 6000.0m,
    Tolerance= 120.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E27",
    Name= "Пыль доменная",
    SourceId= "N07",
    DestinationId= "N17",
    Measured= 700.0m,
    Tolerance= 14.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E28",
    Name= "Кислород на конвертеры",
    SourceId= "N08",
    DestinationId= "N09",
    Measured= 3500.0m,
    Tolerance= 70.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E29",
    Name= "Кислород в конвертер №2",
    SourceId= "N08",
    DestinationId= "N10",
    Measured= 3000.0m,
    Tolerance= 60.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E30",
    Name= "Кислород на резку/сварку",
    SourceId= "N08",
    DestinationId= null,
    Measured= 500.0m,
    Tolerance= 10.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E31",
    Name= "Скрап в конвертер №1",
    SourceId= "N19",
    DestinationId= "N09",
    Measured= 4000.0m,
    Tolerance= 80.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E32",
    Name= "Скрап в конвертер №2",
    SourceId= "N19",
    DestinationId= "N10",
    Measured= 3500.0m,
    Tolerance= 70.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E33",
    Name= "Сталь из конвертера №1",
    SourceId= "N09",
    DestinationId= "N11",
    Measured= 36000.0m,
    Tolerance= 720.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E34",
    Name= "Конвертерный газ №1",
    SourceId= "N09",
    DestinationId= "N17",
    Measured= 4500.0m,
    Tolerance= 90.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E35",
    Name= "Шлак конвертера №1",
    SourceId= "N09",
    DestinationId= "N16",
    Measured= 4000.0m,
    Tolerance= 80.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E36",
    Name= "Сталь из конвертера №2",
    SourceId= "N10",
    DestinationId= "N11",
    Measured= 30000.0m,
    Tolerance= 600.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 100000.0m
  },
                    new VariableDto
  {
    Id= "E37",
    Name= "Конвертерный газ №2",
    SourceId= "N10",
    DestinationId= "N17",
    Measured= 4000.0m,
    Tolerance= 80.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E38",
    Name= "Шлак конвертера №2",
    SourceId= "N10",
    DestinationId= "N16",
    Measured= 3500.0m,
    Tolerance= 70.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E39",
    Name= "Лигатура и ферросплавы",
    SourceId= null,
    DestinationId= "N11",
    Measured= 2500.0m,
    Tolerance= 50.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E40",
    Name= "Сталь доведённая",
    SourceId= "N11",
    DestinationId= "N12",
    Measured= 67000.0m,
    Tolerance= 1340.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 150000.0m
  },
                    new VariableDto
  {
    Id= "E41",
    Name= "Угар и потери на УДС",
    SourceId= "N11",
    DestinationId= null,
    Measured= 1500.0m,
    Tolerance= 30.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E42",
    Name= "Слябы",
    SourceId= "N12",
    DestinationId= "N13",
    Measured= 64000.0m,
    Tolerance= 1280.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 150000.0m
  },
                    new VariableDto
  {
    Id= "E43",
    Name= "Обрезь МНЛЗ",
    SourceId= "N12",
    DestinationId= "N19",
    Measured= 2000.0m,
    Tolerance= 40.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E44",
    Name= "Окалина и потери МНЛЗ",
    SourceId= "N12",
    DestinationId= null,
    Measured= 1000.0m,
    Tolerance= 20.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E45",
    Name= "Горячекатаный рулон",
    SourceId= "N13",
    DestinationId= "N14",
    Measured= 61000.0m,
    Tolerance= 1220.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 150000.0m
  },
                    new VariableDto
  {
    Id= "E46",
    Name= "Обрезь ГП",
    SourceId= "N13",
    DestinationId= "N19",
    Measured= 2500.0m,
    Tolerance= 50.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E47",
    Name= "Окалина ГП",
    SourceId= "N13",
    DestinationId= null,
    Measured= 500.0m,
    Tolerance= 10.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E48",
    Name= "Холоднокатаный лист",
    SourceId= "N14",
    DestinationId= "N15",
    Measured= 59000.0m,
    Tolerance= 1180.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 150000.0m
  },
                    new VariableDto
  {
    Id= "E49",
    Name= "Обрезь ХП",
    SourceId= "N14",
    DestinationId= "N19",
    Measured= 1500.0m,
    Tolerance= 30.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E50",
    Name= "Брак и потери ХП",
    SourceId= "N14",
    DestinationId= null,
    Measured= 500.0m,
    Tolerance= 10.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E51",
    Name= "Готовая продукция (отгрузка)",
    SourceId= "N15",
    DestinationId= null,
    Measured= 59000.0m,
    Tolerance= 1180.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 150000.0m
  },
                    new VariableDto
  {
    Id= "E52",
    Name= "Шлак в переработку",
    SourceId= "N07",
    DestinationId= "N16",
    Measured= 6000.0m,
    Tolerance= 120.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E53",
    Name= "Щебень из шлака",
    SourceId= "N16",
    DestinationId= null,
    Measured= 12000.0m,
    Tolerance= 240.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E54",
    Name= "Извлечённый металл из шлака",
    SourceId= "N16",
    DestinationId= "N19",
    Measured= 1300.0m,
    Tolerance= 26.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E55",
    Name= "Потери при переработке шлака",
    SourceId= "N16",
    DestinationId= null,
    Measured= 200.0m,
    Tolerance= 4.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 20000.0m
  },
                    new VariableDto
  {
    Id= "E56",
    Name= "Уловленная пыль",
    SourceId= "N17",
    DestinationId= "N06",
    Measured= 1200.0m,
    Tolerance= 24.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E57",
    Name= "Сброс в атмосферу",
    SourceId= "N17",
    DestinationId= null,
    Measured= 700.0m,
    Tolerance= 14.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E58",
    Name= "Пар на технологию",
    SourceId= "N18",
    DestinationId= "N07",
    Measured= 1000.0m,
    Tolerance= 20.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E59",
    Name= "Электроэнергия (условная)",
    SourceId= "N18",
    DestinationId= null,
    Measured= 500.0m,
    Tolerance= 10.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  },
                    new VariableDto
  {
    Id= "E60",
    Name= "Вода подпитки",
    SourceId= null,
    DestinationId= "N20",
    Measured= 2000.0m,
    Tolerance= 40.0m,
    IsMeasured= true,
    MinBound= 0.0m,
    MaxBound= 50000.0m
  }
            }
            };

            var result = solver.Solve(request);
            Assert.AreEqual("FOUND_IN_BOUNDS", result.Status);

            // Ответы
            double[] answers = { 13637.8706105788, 13537.9843515934, 99.8862589853765, 10504.5785186866, 1585.0230169635,
                                 1448.38281594327, 5103.54259007098, 247.620053549909, 49.8732849947691, 49957.8429157833,
                                 49459.3988287389, 498.444087044409, 8393.08202174225, 8193.32769800584, 199.754323736411,
                                 29263.2330592732, 728.639600048164, 149.881572725635, 5103.54259007098, 29263.2330592731,
                                 -1.87181050778079e-14, 2018.54071212154, 33322.7522478486, 27814.9282349436, 0,
                                 5041.5566588706, 600.243659453275, 1.39974563027273e-13, -5.54157110285012e-14, 1.41853102069131e-13,
                                 4015.73920796371, 3521.99657446195, 33520.8828121112, 302.654613053677, 3514.9540306474,
                                 27547.6464312737, 670.587941782206, 3118.69043634969, 2540.08891192319, 62123.0501636004,
                                 1485.56799170765, 59120.3154835934, 2008.06248358534, 994.672196421628, 56101.3061639062,
                                 2520.04343827484, 498.965881412317, 54091.8333965048, 1510.17776116814, 499.295006233339,
                                 54091.8333965048, 5041.55665887061, 15166.3077281421, 1349.5705266717, 200.879529924484,
                                 1504.64071102549, 797.485103311836, 1068.94544917687, 516.077567786622, 0 };
            List<string> grossErrorStreams = new List<string> { "Leak_between_N17_and_N08", "Leak_between_N18_and_N06",
                "Leak_between_N20_and_N03", "Leak_between_N16_and_N03", "E16", "Leak_between_N15_and_N09", "Leak_between_N08_and_N04", "E24",
                "E17", "Leak_between_N08_and_N01", "Leak_between_N15_and_N08", "Leak_between_N19_and_N05", "Leak_between_N09_and_N05", "E54"
            };
            for (int i = 0; i < result.ReconciledVariables.Count; i++)
            {
                double diff = Math.Abs((double)result.ReconciledVariables[i].ReconciledValue - answers[i]);
                Assert.True(diff < 1e-13, $"Поток x{i + 1}= ожидаемое значение {answers[i]}, полученное значение {result.ReconciledVariables[i].ReconciledValue}, разница {diff}");
            }

            Assert.AreEqual(grossErrorStreams, result.GrossErrorStreams);
        }

    }
}

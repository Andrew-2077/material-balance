using MaterialBalanceAPI.Services;
using MaterialBalanceAPI.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Core.Pipeline;
using MaterialBalanceAPI.Data.Entities;

namespace MaterialBalanceAPI.Decorators
{
    public class LoggingDataOrchestratorServiceDecorator : IDataOrchestratorService
    {
        private readonly IDataOrchestratorService _innerService;
        private readonly ILogger<LoggingDataOrchestratorServiceDecorator> _logger;

        public LoggingDataOrchestratorServiceDecorator(
            IDataOrchestratorService innerService,
            ILogger<LoggingDataOrchestratorServiceDecorator> logger)
        {
            _innerService = innerService;
            _logger = logger;
        }

        public async Task<BalanceRequest> GetBalanceDataForPeriodAsync(int periodId)
        {
            _logger.LogInformation("Начало загрузки данных для сведения баланса для периода ID: {PeriodId}", periodId);

            Stopwatch sw = Stopwatch.StartNew();
            var result = await _innerService.GetBalanceDataForPeriodAsync(periodId);
            sw.Stop();

            _logger.LogInformation("Загрузка данных для сведения баланса для периода ID: {PeriodId} завершена за {ElapsedMs} мс. Потоков найдено: {Count}",
                periodId, sw.ElapsedMilliseconds, result?.Variables?.Count ?? 0);
            return result;
        }

        public async Task<List<VariableDto>> GetPlantModelAsync()
        {
            _logger.LogInformation("Начало загрузки модели завода...");

            Stopwatch sw = Stopwatch.StartNew();
            var result = await _innerService.GetPlantModelAsync();
            sw.Stop();

            _logger.LogInformation("Модель завода успешно загружена за {ElapsedMs} мс. Элементов: {Count}",
                sw.ElapsedMilliseconds, result?.Count ?? 0);
            return result;
        }

        public async Task<List<PeriodsDto>> GetPeriodsAsync()
        {
            _logger.LogInformation("Загрузка периодов работы завода...");

            Stopwatch sw = Stopwatch.StartNew();
            var result = await _innerService.GetPeriodsAsync();
            sw.Stop();

            _logger.LogInformation("Периоды успешно загружены за {ElapsedMs} мс. Элементов: {Count}",
            sw.ElapsedMilliseconds, result?.Count ?? 0);
            return result;
        }
    }
}

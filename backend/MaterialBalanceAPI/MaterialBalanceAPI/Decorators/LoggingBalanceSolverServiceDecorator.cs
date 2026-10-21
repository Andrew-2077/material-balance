using MaterialBalanceAPI.Services;
using MaterialBalanceAPI.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MaterialBalanceAPI.Decorators
{
    public class LoggingBalanceSolverServiceDecorator : IBalanceSolverService
    {
        private readonly IBalanceSolverService _innerService;
        private readonly ILogger<LoggingBalanceSolverServiceDecorator> _logger;

        public LoggingBalanceSolverServiceDecorator(
            IBalanceSolverService innerService,
            ILogger<LoggingBalanceSolverServiceDecorator> logger)
        {
            _innerService = innerService;
            _logger = logger;
        }

        public BalanceResponse Solve(BalanceRequest request)
        {
            _logger.LogInformation("Запуск расчета баланса. Потоков найдено: {Count}", request.Variables?.Count ?? 0);
            
            var sw = Stopwatch.StartNew();
            var response = _innerService.Solve(request);
            sw.Stop();

            _logger.LogInformation("Расчет успешно завершен со статусом: {Status}. Общее время: {TotalMs} мс",
                response.Status, sw.ElapsedMilliseconds);
            _logger.LogInformation("Время, затраченное на расчет баланса: {TimeMs} мс", response.TimeMs);
            _logger.LogInformation("Время, затраченное на поиск грубых ошибок: {TimeDetectErrorsMs} мс", response.TimeDetectErrorsMs);
            return response;
        }
    }
}

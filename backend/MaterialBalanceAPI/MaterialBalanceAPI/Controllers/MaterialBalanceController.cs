using MaterialBalanceAPI.Models;
using MaterialBalanceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaterialBalanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceSolverService _solver;
        private readonly ILogger<BalanceController> _logger;

        public BalanceController(IBalanceSolverService solver, ILogger<BalanceController> logger)
        {
            _solver = solver;
            _logger = logger;
        }

        [HttpPost("solve")]
        public ActionResult<BalanceResponse> Solve([FromBody] BalanceRequest request)
        {
            _logger.LogInformation("Получен запрос на расчет баланса. Количество потоков: {Count}", request.Variables.Count);

            try
            {
                var response = _solver.Solve(request);
                _logger.LogInformation("Расчет успешно завершен со статусом: {Status}", response.Status);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при расчете баланса");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}

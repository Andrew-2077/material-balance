using MaterialBalanceAPI.Models;
using MaterialBalanceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

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
            try
            {
                var response = _solver.Solve(request);
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

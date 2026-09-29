using Microsoft.AspNetCore.Mvc;
using MaterialBalanceAPI.Services;
using MaterialBalanceAPI.Models;
using System.Threading.Tasks;

namespace MaterialBalanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantOperationsController : ControllerBase
    {
        private readonly IDataOrchestratorService _dataService;
        private readonly IBalanceSolverService _solverService;

        public PlantOperationsController(IDataOrchestratorService dataService, IBalanceSolverService solverService)
        {
            _dataService = dataService;
            _solverService = solverService;
        }

        // Возвращает структуру завода (узлы и связи) без замеров для отрисовки на фронтенде.
        [HttpGet("Model")]
        public async Task<IActionResult> GetPlantModel()
        {
            var model = await _dataService.GetPlantModelAsync();
            return Ok(model);
        }

        // Выполняет расчет сведения баланса для конкретного периода.
        [HttpPost("calculate/{periodId}")]
        public async Task<IActionResult> CalculateBalance(int periodId)
        {
            Console.WriteLine($"Запрос на расчет для периода: {periodId}");

            var variables = await _dataService.GetBalanceDataForPeriodAsync(periodId);

            if (variables == null || variables.Variables.Count == 0)
            {
                return BadRequest(new { message = "Нет данных для указанного периода или структура завода пуста." });
            }

            var request = new BalanceRequest
            {
                Variables = variables.Variables
            };

            var response = _solverService.Solve(request);
            return Ok(response);
        }
    }
}

using MaterialBalanceAPI.Data;
using MaterialBalanceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialBalanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeriodsController : ControllerBase
    {
        private readonly IDataOrchestratorService _dataService;

        public PeriodsController(IDataOrchestratorService dataService) => _dataService = dataService;

        [HttpGet]
        public async Task<IActionResult> GetPeriods()
        {
            // Возвращаем список ID и названий периодов для выпадающего списка на фронте
            var periods = await _dataService.GetPeriodsAsync();
            return Ok(periods);
        }
    }
}

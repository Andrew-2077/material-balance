using MaterialBalanceAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaterialBalanceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeriodsController : ControllerBase
    {
        private readonly PlantDbContext _context;

        public PeriodsController(PlantDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetPeriods()
        {
            // Возвращаем список ID и названий периодов для выпадающего списка на фронте
            var periods = await _context.Periods
                .Select(p => new { p.Id, p.Name, p.Timestamp })
                .ToListAsync();
            return Ok(periods);
        }
    }
}

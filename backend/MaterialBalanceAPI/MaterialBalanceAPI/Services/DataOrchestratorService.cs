using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MaterialBalanceAPI.Data;
using MaterialBalanceAPI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialBalanceAPI.Services
{
    public interface IDataOrchestratorService
    {
        Task<BalanceRequest> GetBalanceDataForPeriodAsync(int periodId);
        Task<List<VariableDto>> GetPlantModelAsync();
        Task<List<PeriodsDto>> GetPeriodsAsync();
    }
    public class DataOrchestratorService : IDataOrchestratorService
    {
        private readonly PlantDbContext _context;

        public DataOrchestratorService(PlantDbContext context)
        {
            _context = context;
        }
        public DataOrchestratorService() { }

        public async Task<BalanceRequest> GetBalanceDataForPeriodAsync(int periodId)
        {
            // Получаем все потоки (модель завода)
            var streams = await _context.Streams.ToListAsync();

            // Получаем измерения для конкретного периода
            var measurements = await _context.Measurements
                .Where(m => m.PeriodId == periodId)
                .ToListAsync();

            var request = new BalanceRequest
            {
                Variables = new List<VariableDto>()
            };

            // Собираем данные воедино для солвера
            foreach (var stream in streams)
            {
                var measurement = measurements.FirstOrDefault(m => m.StreamId == stream.Id);
                if (measurement != null)
                {
                    request.Variables.Add(new VariableDto
                    {
                        Id = stream.Id,
                        Name = stream.Name,
                        SourceId = stream.SourceNodeId,
                        DestinationId = stream.DestinationNodeId,
                        Measured = measurement.Value,
                        Tolerance = measurement.Tolerance,
                        IsMeasured = measurement.IsMeasured,
                        MinBound = stream.MinBound,
                        MaxBound = stream.MaxBound
                    });
                }
            }

            return request;
        }

        public async Task<List<VariableDto>> GetPlantModelAsync()
        {
            // Метод для фронтенда: возвращает только структуру завода
            var streams = await _context.Streams.ToListAsync();
            return streams.Select(s => new VariableDto
            {
                Id = s.Id,
                Name = s.Name,
                SourceId = s.SourceNodeId,
                DestinationId = s.DestinationNodeId,
                MinBound = s.MinBound,
                MaxBound = s.MaxBound,
                IsMeasured = false // Для пустой модели замеры не важны
            }).ToList();
        }

        public async Task<List<PeriodsDto>> GetPeriodsAsync()
        {
            var periods = await _context.Periods.ToListAsync();
            return periods.Select(p => new PeriodsDto
            {
                Id = p.Id,
                Name = p.Name,
                Timestamp = p.Timestamp,
            }).ToList();
        }
    }
}

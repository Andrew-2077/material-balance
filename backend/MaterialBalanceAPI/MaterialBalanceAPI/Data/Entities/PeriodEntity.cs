using System.Collections.Generic;

namespace MaterialBalanceAPI.Data.Entities
{
    // Модель периода (например, день, смена)
    public class PeriodEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Timestamp { get; set; }

        // Один-ко-многим с измерениями
        public List<MeasurementEntity>? Measurements { get; set; }
    }
}

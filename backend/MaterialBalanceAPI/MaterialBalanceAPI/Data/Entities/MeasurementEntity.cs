namespace MaterialBalanceAPI.Data.Entities
{
    // Данные с датчиков (Измерения для конкретного периода)
    public class MeasurementEntity
    {
        public int Id { get; set; }

        // Внешние ключи
        public int PeriodId { get; set; }
        public PeriodEntity? Period { get; set; }

        public string? StreamId { get; set; }
        public StreamEntity? Stream { get; set; }

        // Динамические данные датчика
        public decimal Value { get; set; }
        public decimal Tolerance { get; set; }
        public bool IsMeasured { get; set; }
    }
}

namespace MaterialBalanceAPI.Models
{
    public class VariableDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? SourceId { get; set; } // null если входной поток
        public string? DestinationId { get; set; } // null если выходной поток
        public decimal Measured { get; set; }
        public decimal Tolerance { get; set; }
        public bool IsMeasured { get; set; }
        public decimal MinBound { get; set; }
        public decimal MaxBound { get; set; }
    }
}

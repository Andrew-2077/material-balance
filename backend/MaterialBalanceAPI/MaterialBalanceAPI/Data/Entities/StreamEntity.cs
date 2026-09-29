using System.Collections.Generic;

namespace MaterialBalanceAPI.Data.Entities
{
    // Модель завода (топология)
    public class StreamEntity
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? SourceNodeId { get; set; } // null для входного потока
        public string? DestinationNodeId { get; set; } // null для выходного потока
        
        // Физические ограничения
        public decimal MinBound { get; set; }
        public decimal MaxBound { get; set; }
    }
}

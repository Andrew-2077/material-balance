namespace MaterialBalanceAPI.Models
{
    public class BalanceResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal ReconciledValue { get; set; }
        public decimal Measured { get; set; }
        public bool isMeasured { get; set; }
    }

    public class BalanceResponse
    {
        public List<BalanceResult> ReconciledVariables { get; set; }
        public string Status { get; set; }
        public decimal GlobalTestValue { get; set; }
        public bool IsGlobalTestPassed { get; set; }
        public List<string> GrossErrorStreams { get; set; }
        public long TimeMs { get; set; }
        public long TimeDetectErrorsMs { get; set; }
    }
}

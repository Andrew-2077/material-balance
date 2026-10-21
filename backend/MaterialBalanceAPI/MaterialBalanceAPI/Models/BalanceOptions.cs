namespace MaterialBalanceAPI.Models
{
    public class BalanceOptions
    {
        public double CoefDelta { get; set; } = 1.96;
        public double Alpha { get; set; } = 0.05;
    }
}

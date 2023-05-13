public class Calculation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public double InitialInvestment { get; set; }
    public double DiscountRate { get; set; }
    public double InflationRate { get; set; }
    public double TaxRate { get; set; }
    public int PoliticalStabilityRating { get; set; }
    public double NPV { get; set; }
}

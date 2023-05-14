public class Calculation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal InitialInvestment { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal InflationRate { get; set; }
    public decimal TaxRate { get; set; }
    public int PoliticalStabilityRating { get; set; }
    public decimal NPV { get; set; }
}
namespace Architypes.Core.Configuration;

public class PricingSettings
{
    public const string SectionName = "Pricing";

    public decimal FullReportPrice { get; set; }
    public required string Currency { get; set; }
}

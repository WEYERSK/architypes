namespace Architypes.Core.Configuration;

public class PayFastSettings
{
    public const string SectionName = "PayFast";

    public required string MerchantId { get; set; }
    public required string MerchantKey { get; set; }
    public required string Passphrase { get; set; }
    public required string ProcessUrl { get; set; }
    public required string ValidateUrl { get; set; }
    public required string ReturnUrl { get; set; }
    public required string CancelUrl { get; set; }
    public required string NotifyUrl { get; set; }
}

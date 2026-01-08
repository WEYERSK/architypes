using System.Security.Cryptography;
using System.Text;
using Architypes.Application.Interfaces;
using Architypes.Core.Configuration;
using Architypes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Architypes.Application.Services;

public class PayFastPaymentService : IPaymentService
{
    private readonly ArchitypesDbContext _context;
    private readonly PayFastSettings _payFastSettings;
    private readonly PricingSettings _pricingSettings;

    public PayFastPaymentService(
        ArchitypesDbContext context,
        IOptions<PayFastSettings> payFastSettings,
        IOptions<PricingSettings> pricingSettings)
    {
        _context = context;
        _payFastSettings = payFastSettings.Value;
        _pricingSettings = pricingSettings.Value;
    }

    public async Task<string> GeneratePaymentFormAsync(Guid sessionId, string returnUrl, string cancelUrl, string notifyUrl)
    {
        var assessment = await _context.Assessments
            .FirstOrDefaultAsync(a => a.SessionId == sessionId);

        if (assessment == null)
        {
            throw new ArgumentException("Assessment not found");
        }

        var paymentData = new Dictionary<string, string>
        {
            { "merchant_id", _payFastSettings.MerchantId },
            { "merchant_key", _payFastSettings.MerchantKey },
            { "return_url", returnUrl },
            { "cancel_url", cancelUrl },
            { "notify_url", notifyUrl },
            { "amount", _pricingSettings.FullReportPrice.ToString("F2") },
            { "item_name", "Full Archetype Report" },
            { "item_description", $"Complete archetype analysis for assessment {sessionId}" },
            { "email_address", assessment.Email ?? "" },
            { "custom_str1", sessionId.ToString() }
        };

        // Generate signature
        var signature = GenerateSignature(paymentData);
        paymentData.Add("signature", signature);

        // Generate HTML form
        var formHtml = new StringBuilder();
        formHtml.AppendLine($"<form action=\"{_payFastSettings.ProcessUrl}\" method=\"post\" id=\"payfast-form\">");

        foreach (var kvp in paymentData)
        {
            formHtml.AppendLine($"<input type=\"hidden\" name=\"{kvp.Key}\" value=\"{kvp.Value}\" />");
        }

        formHtml.AppendLine("</form>");
        formHtml.AppendLine("<script>document.getElementById('payfast-form').submit();</script>");

        return formHtml.ToString();
    }

    public async Task<bool> ValidatePaymentNotificationAsync(Dictionary<string, string> paymentData)
    {
        if (!paymentData.ContainsKey("signature"))
        {
            return false;
        }

        var receivedSignature = paymentData["signature"];
        paymentData.Remove("signature");

        var calculatedSignature = GenerateSignature(paymentData);

        return receivedSignature == calculatedSignature;
    }

    public async Task ProcessSuccessfulPaymentAsync(Guid sessionId, string paymentReference)
    {
        var assessment = await _context.Assessments
            .FirstOrDefaultAsync(a => a.SessionId == sessionId);

        if (assessment != null)
        {
            assessment.HasPurchasedFullReport = true;
            assessment.PaymentReference = paymentReference;
            await _context.SaveChangesAsync();
        }
    }

    private string GenerateSignature(Dictionary<string, string> data)
    {
        // Sort parameters alphabetically
        var sortedData = data.OrderBy(x => x.Key);

        // Create parameter string
        var paramString = string.Join("&", sortedData.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));

        // Add passphrase if configured
        if (!string.IsNullOrEmpty(_payFastSettings.Passphrase))
        {
            paramString += $"&passphrase={Uri.EscapeDataString(_payFastSettings.Passphrase)}";
        }

        // Generate MD5 hash
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(paramString));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}

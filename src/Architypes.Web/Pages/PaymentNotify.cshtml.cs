using Architypes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Architypes.Web.Pages;

public class PaymentNotifyModel : PageModel
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentNotifyModel> _logger;

    public PaymentNotifyModel(IPaymentService paymentService, ILogger<PaymentNotifyModel> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            // Get all form data from PayFast
            var paymentData = new Dictionary<string, string>();
            foreach (var key in Request.Form.Keys)
            {
                paymentData[key] = Request.Form[key].ToString();
            }

            // Log the notification
            _logger.LogInformation("PayFast notification received: {Data}",
                string.Join(", ", paymentData.Select(x => $"{x.Key}={x.Value}")));

            // Validate the payment notification
            var isValid = await _paymentService.ValidatePaymentNotificationAsync(paymentData);

            if (!isValid)
            {
                _logger.LogWarning("Invalid PayFast notification signature");
                return BadRequest("Invalid signature");
            }

            // Check payment status
            var paymentStatus = paymentData.GetValueOrDefault("payment_status", "");
            if (paymentStatus != "COMPLETE")
            {
                _logger.LogInformation("Payment not complete. Status: {Status}", paymentStatus);
                return new OkResult();
            }

            // Extract session ID from custom field
            var sessionIdStr = paymentData.GetValueOrDefault("custom_str1", "");
            if (!Guid.TryParse(sessionIdStr, out var sessionId))
            {
                _logger.LogWarning("Invalid session ID in payment notification: {SessionId}", sessionIdStr);
                return BadRequest("Invalid session ID");
            }

            // Get payment reference
            var paymentReference = paymentData.GetValueOrDefault("pf_payment_id", "");

            // Process the successful payment
            await _paymentService.ProcessSuccessfulPaymentAsync(sessionId, paymentReference);

            _logger.LogInformation("Payment processed successfully for session {SessionId}, reference {Reference}",
                sessionId, paymentReference);

            return new OkResult();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PayFast notification");
            return StatusCode(500, "Internal server error");
        }
    }
}

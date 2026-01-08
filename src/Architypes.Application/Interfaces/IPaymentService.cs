namespace Architypes.Application.Interfaces;

public interface IPaymentService
{
    Task<string> GeneratePaymentFormAsync(Guid sessionId, string returnUrl, string cancelUrl, string notifyUrl);
    Task<bool> ValidatePaymentNotificationAsync(Dictionary<string, string> paymentData);
    Task ProcessSuccessfulPaymentAsync(Guid sessionId, string paymentReference);
}

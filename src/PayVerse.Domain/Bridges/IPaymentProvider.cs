using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.Bridges;

/// <summary>
/// The Implementor interface for payment processing
/// </summary>
public interface IPaymentProvider
{
    Task<string> ProcessPaymentAsync(Guid paymentId, decimal amount, string currency, IDictionary<string, string> paymentDetails);
    Task<bool> RefundPaymentAsync(string transactionId, decimal amount, string currency);
    Task<PaymentStatus> CheckPaymentStatusAsync(string transactionId);
    bool SupportsRecurringPayments();
    bool SupportsPartialRefunds();
    string GetProviderName();
}

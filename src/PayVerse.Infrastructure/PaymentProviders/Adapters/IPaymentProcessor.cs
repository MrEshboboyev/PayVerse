using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Infrastructure.PaymentProviders.Adapters;

// Define the target interface that our application expects
public interface IPaymentProcessor
{
    Task<PaymentProcessResult> ProcessPaymentAsync(Payment payment);
    Task<RefundProcessResult> ProcessRefundAsync(Payment payment);
    Task<PaymentVerificationResult> VerifyPaymentAsync(string transactionId);
}

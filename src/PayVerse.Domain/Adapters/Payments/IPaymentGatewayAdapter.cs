using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Domain.Adapters.Payments;

// Define the target interface that our application expects
public interface IPaymentGatewayAdapter
{
    Task<PaymentProcessResult> ProcessPaymentAsync(Payment payment);
    Task<RefundProcessResult> ProcessRefundAsync(Payment payment);
    Task<PaymentVerificationResult> VerifyPaymentAsync(string transactionId);
}

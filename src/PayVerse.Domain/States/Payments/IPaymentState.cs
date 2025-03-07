using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.States.Payments;

public interface IPaymentState
{
    PaymentStatus Status { get; }
    Task ProcessAsync(Payment payment, string transactionId, string providerName);
    Task RefundAsync(Payment payment, string refundTransactionId);
    Task CancelAsync(Payment payment, string reason);
    Task FailAsync(Payment payment, string reason);
}
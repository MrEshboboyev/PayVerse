using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.States.Payments.ConcreteStates;

public class RefundedPaymentState : IPaymentState
{
    public PaymentStatus Status => PaymentStatus.Refunded;

    public async Task ProcessAsync(Payment payment, string transactionId, string providerName)
    {
        // Cannot process a refunded payment
        throw new InvalidOperationException("Cannot process a payment that has been refunded.");
    }

    public async Task RefundAsync(Payment payment, string refundTransactionId)
    {
        // Already refunded
        throw new InvalidOperationException("Payment has already been refunded.");
    }

    public async Task CancelAsync(Payment payment, string reason)
    {
        // Cannot cancel a refunded payment
        throw new InvalidOperationException("Cannot cancel a payment that has been refunded.");
    }

    public async Task FailAsync(Payment payment, string reason)
    {
        // A refunded payment cannot fail
        throw new InvalidOperationException("Cannot fail a payment that has been refunded.");
    }
}

using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.States.Payments.ConcreteStates;

public class CancelledPaymentState : IPaymentState
{
    public PaymentStatus Status => PaymentStatus.Cancelled;

    public Task ProcessAsync(Payment payment, string transactionId, string providerName)
    {
        // Cannot process a cancelled payment
        throw new InvalidOperationException("Cannot process a payment that has been cancelled.");
    }

    public Task RefundAsync(Payment payment, string refundTransactionId)
    {
        // Cannot refund a cancelled payment
        throw new InvalidOperationException("Cannot refund a payment that has been cancelled.");
    }

    public Task CancelAsync(Payment payment, string reason)
    {
        // Already cancelled
        throw new InvalidOperationException("Payment has already been cancelled.");
    }

    public Task FailAsync(Payment payment, string reason)
    {
        // A cancelled payment cannot fail
        throw new InvalidOperationException("Cannot fail a payment that has been cancelled.");
    }
}

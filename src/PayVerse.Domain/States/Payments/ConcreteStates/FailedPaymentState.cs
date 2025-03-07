using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.States.Payments.ConcreteStates;

public class FailedPaymentState : IPaymentState
{
    public PaymentStatus Status => PaymentStatus.Failed;

    public async Task ProcessAsync(Payment payment, string transactionId, string providerName)
    {
        // Cannot process a failed payment
        throw new InvalidOperationException("Cannot process a payment that has failed.");
    }

    public async Task RefundAsync(Payment payment, string refundTransactionId)
    {
        // Cannot refund a failed payment
        throw new InvalidOperationException("Cannot refund a payment that has failed.");
    }

    public async Task CancelAsync(Payment payment, string reason)
    {
        // Cannot cancel a failed payment
        throw new InvalidOperationException("Cannot cancel a payment that has failed.");
    }

    public async Task FailAsync(Payment payment, string reason)
    {
        // Already failed
        throw new InvalidOperationException("Payment has already failed.");
    }
}
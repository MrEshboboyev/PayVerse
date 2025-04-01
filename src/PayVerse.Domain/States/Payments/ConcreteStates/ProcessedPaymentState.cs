using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.States.Payments.ConcreteStates;

public class ProcessedPaymentState : IPaymentState
{
    public PaymentStatus Status => PaymentStatus.Processed;

    public Task ProcessAsync(Payment payment, string transactionId, string providerName)
    {
        // Already processed
        throw new InvalidOperationException("Payment has already been processed.");
    }

    public async Task RefundAsync(Payment payment, string refundTransactionId)
    {
        //// Can refund a processed payment
        //payment.SetRefundTransactionId(refundTransactionId);
        //payment.SetRefundedDate(DateTime.UtcNow);
        //payment.ChangeState(new RefundedPaymentState());

        // Notify observers
        await payment.NotifyAsync();
    }

    public Task CancelAsync(Payment payment, string reason)
    {
        // Cannot cancel a payment that has been processed
        throw new InvalidOperationException("Cannot cancel a payment that has already been processed.");
    }

    public Task FailAsync(Payment payment, string reason)
    {
        // A processed payment cannot fail
        throw new InvalidOperationException("Cannot fail a payment that has already been processed.");
    }
}

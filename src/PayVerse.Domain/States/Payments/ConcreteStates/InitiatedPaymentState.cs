using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.States.Payments.ConcreteStates;

public class InitiatedPaymentState : IPaymentState
{
    public PaymentStatus Status => PaymentStatus.Initiated;

    public async Task ProcessAsync(Payment payment, string transactionId, string providerName)
    {
        // Transition to Processed state
        payment.SetTransactionId(transactionId);
        payment.SetProviderName(providerName);
        //payment.SetProcessedDate(DateTime.UtcNow);
        //payment.ChangeState(new ProcessedPaymentState());

        // Notify observers
        await payment.NotifyAsync();
    }

    public Task RefundAsync(Payment payment, string refundTransactionId)
    {
        // Cannot refund a payment that hasn't been processed
        throw new InvalidOperationException("Cannot refund a payment that hasn't been processed yet.");
    }

    public async Task CancelAsync(Payment payment, string reason)
    {
        //// Can cancel an initiated payment
        //payment.SetFailureReason(reason);
        //payment.SetCancelledDate(DateTime.UtcNow);
        //payment.ChangeState(new CancelledPaymentState());

        // Notify observers
        await payment.NotifyAsync();
    }

    public async Task FailAsync(Payment payment, string reason)
    {
        //// An initiated payment can fail
        //payment.SetFailureReason(reason);
        //payment.ChangeState(new FailedPaymentState());

        // Notify observers
        await payment.NotifyAsync();
    }
}
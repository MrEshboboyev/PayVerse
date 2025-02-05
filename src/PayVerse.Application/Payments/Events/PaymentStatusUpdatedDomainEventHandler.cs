using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Payments;
using PayVerse.Domain.Repositories.Payments;

namespace PayVerse.Application.Payments.Events;

internal sealed class PaymentStatusUpdatedDomainEventHandler(
    IPaymentRepository paymentRepository) : IDomainEventHandler<PaymentStatusUpdatedDomainEvent>
{
    public async Task Handle(
        PaymentStatusUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        #region Get this payment
        
        var payment = await paymentRepository.GetByIdAsync(notification.PaymentId, cancellationToken);
        if (payment is null)
        {
            return;
        }
        
        #endregion
        
        Console.WriteLine($"Payment [ID : {payment.Id}] status updated" +
                          $" from {notification.OldStatus} to {notification.NewStatus}");
    }
}
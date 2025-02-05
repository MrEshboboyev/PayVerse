using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Payments;
using PayVerse.Domain.Repositories.Payments;

namespace PayVerse.Application.Payments.Events;

internal sealed class PaymentInitiatedDomainEventHandler(
    IPaymentRepository paymentRepository) : IDomainEventHandler<PaymentInitiatedDomainEvent>
{
    public async Task Handle(
        PaymentInitiatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        #region Get this payment
        
        var payment = await paymentRepository.GetByIdAsync(notification.PaymentId, cancellationToken);
        if (payment is null)
        {
            return;
        }
        
        #endregion
        
        Console.WriteLine($"Payment id: {payment.Id} has been initiated");
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Payments;
using PayVerse.Domain.Repositories.Payments;

namespace PayVerse.Application.Payments.Events;

internal sealed class PaymentRetryInitiatedDomainEventHandler(
    IPaymentRepository paymentRepository) : IDomainEventHandler<PaymentRetryInitiatedDomainEvent>
{
    public async Task Handle(
        PaymentRetryInitiatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetByIdAsync(notification.PaymentId, cancellationToken);
        if (payment is null)
        {
            return;
        }

        Console.WriteLine($"Payment id: {payment.Id} retry initiated");
    }
}
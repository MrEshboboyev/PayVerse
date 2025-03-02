using PayVerse.Application.Common.Interfaces;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Application.Payments.Decorators;

/// <summary>
/// Adds notification capabilities to payment processing
/// </summary>
public class NotificationPaymentDecorator(
    IPaymentProcessor paymentProcessor,
    INotificationService notificationService) : PaymentProcessorDecorator(paymentProcessor)
{
    public override async Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        var result = await base.ProcessPaymentAsync(payment, cancellationToken);

        if (result.Success)
        {
            // Send payment confirmation notification
            await notificationService.SendNotificationAsync(
                payment.UserId,
                NotificationMessage.Create(
                    $"Your payment of {payment.Amount.Value} 'USD' (FIX THIS COMING SOON) has been processed successfully.")
                .Value,
                NotificationType.PaymentConfirmation,
                NotificationPriority.Medium,
                NotificationDeliveryMethod.Email,
                cancellationToken);
        }

        return result;
    }
}

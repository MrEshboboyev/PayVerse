using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Notifications;

namespace PayVerse.Application.Notifications.Events;

internal sealed class NotificationSentDomainEventHandler : IDomainEventHandler<NotificationSentDomainEvent>
{
    public async Task Handle(NotificationSentDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Notification Sent: {notification.NotificationId}");
    }
}
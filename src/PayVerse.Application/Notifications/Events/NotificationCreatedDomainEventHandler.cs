using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Notifications;

namespace PayVerse.Application.Notifications.Events;

internal sealed class NotificationCreatedDomainEventHandler : IDomainEventHandler<NotificationCreatedDomainEvent>
{
    public async Task Handle(
        NotificationCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Notification Created: {notification.NotificationId}");
    }
}

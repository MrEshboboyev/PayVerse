using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Notifications;

namespace PayVerse.Application.Notifications.Commands.CreateNotification;

public sealed record CreateNotificationCommand(
    string NotificationMessage,
    NotificationType Type,
    NotificationPriority Priority,
    Guid UserId,
    NotificationDeliveryMethod DeliveryMethod) : ICommand;
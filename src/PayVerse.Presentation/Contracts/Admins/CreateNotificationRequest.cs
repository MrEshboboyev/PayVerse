using PayVerse.Domain.Enums.Notifications;

namespace PayVerse.Presentation.Contracts.Admins;

public sealed record CreateNotificationRequest(
    string Message,
    NotificationType Type,
    NotificationPriority Priority,
    Guid UserId,
    NotificationDeliveryMethod DeliveryMethod);
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Application.Notifications.Queries.Common.Responses;

public sealed record NotificationResponse(
    Guid NotificationId,
    NotificationMessage NotificationMessage,
    NotificationType NotificationType,
    NotificationPriority NotificationPriority,
    NotificationStatus NotificationStatus,
    Guid UserId,
    NotificationDeliveryMethod DeliveryMethod,
    bool IsRead,
    bool IsSent,
    DateTime? SentAt,
    DateTime? ReadAt,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);
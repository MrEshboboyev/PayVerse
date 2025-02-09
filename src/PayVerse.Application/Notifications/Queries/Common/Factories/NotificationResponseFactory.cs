using PayVerse.Application.Notifications.Queries.Common.Responses;
using PayVerse.Domain.Entities.Notifications;

namespace PayVerse.Application.Notifications.Queries.Common.Factories;

public static class NotificationResponseFactory
{
    public static NotificationResponse Create(Notification notification)
    {
        return new NotificationResponse(
            notification.Id,
            notification.Message,
            notification.Type,
            notification.Priority,
            notification.Status,
            notification.UserId,
            notification.DeliveryMethod,
            notification.IsRead,
            notification.IsSent,
            notification.SentAt,
            notification.ReadAt,
            notification.CreatedOnUtc,
            notification.ModifiedOnUtc);
    }
}
using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Notifications;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Infrastructure.Services;

/// <summary>
/// Implementation of notification service
/// </summary>
public class NotificationService(
    INotificationRepository notificationRepository,
    ILogger<NotificationService> logger) : INotificationService
{
    public async Task<Guid> SendNotificationAsync(
        Guid userId,
        NotificationMessage message,
        NotificationType type,
        NotificationPriority priority,
        NotificationDeliveryMethod deliveryMethod,
        CancellationToken cancellationToken = default)
    {
        // Create notification using factory method (matching the domain pattern)
        var notification = Notification.Create(
            Guid.NewGuid(),
            message,
            type,
            priority,
            NotificationStatus.Pending,
            userId,
            deliveryMethod);

        await notificationRepository.AddAsync(notification, cancellationToken);

        // In a real implementation, we would also send the notification through the appropriate channel
        // based on the delivery method (email, SMS, push notification, etc.)

        logger.LogInformation("Notification sent to user {UserId} via {DeliveryMethod}: {Message}",
            userId, deliveryMethod, message);

        return notification.Id;
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await notificationRepository.GetAllAsync(cancellationToken);
        return notifications.Where(n => n.UserId == userId && !n.IsRead);
    }

    public async Task<bool> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await notificationRepository.GetByIdAsync(notificationId, cancellationToken);
        if (notification == null)
        {
            logger.LogWarning("Attempt to mark non-existent notification as read: {NotificationId}", notificationId);
            return false;
        }

        // In a proper DDD implementation, this would be done through a domain method on the Notification aggregate
        // This is simplified for the example

        // Update notification as read (implementation would depend on the repository pattern being used)

        logger.LogInformation("Notification {NotificationId} marked as read", notificationId);

        notification.MarkAsRead();
        await notificationRepository.UpdateAsync(notification, cancellationToken);
        return true;
    }
}
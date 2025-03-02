using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Application.Common.Interfaces;

/// <summary>
/// Service for sending notifications to users
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends a notification to a user
    /// </summary>
    /// <param name="userId">ID of the user to notify</param>
    /// <param name="message">Notification message</param>
    /// <param name="type">Type of notification</param>
    /// <param name="priority">Priority of the notification</param>
    /// <param name="deliveryMethod">Method of delivery</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ID of the created notification</returns>
    Task<Guid> SendNotificationAsync(
        Guid userId,
        NotificationMessage message,
        NotificationType type,
        NotificationPriority priority,
        NotificationDeliveryMethod deliveryMethod,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets unread notifications for a user
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of unread notifications</returns>
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks a notification as read
    /// </summary>
    /// <param name="notificationId">ID of the notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the notification was successfully marked as read</returns>
    Task<bool> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
}
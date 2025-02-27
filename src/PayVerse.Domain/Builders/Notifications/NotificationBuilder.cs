using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.ValueObjects.Notifications;

namespace PayVerse.Domain.Builders.Notifications;

/// <summary>
/// Builder for creating Notification entities
/// </summary>
/// <remarks>
/// Constructor with required parameters
/// </remarks>
public class NotificationBuilder(Guid userId,
                                 string message,
                                 NotificationType type) : IBuilder<Notification>
{
    #region Private Properties

    private readonly NotificationMessage _message = NotificationMessage.Create(message).Value;

    // Optional parameters with default values
    private NotificationPriority _priority = NotificationPriority.Medium;
    private NotificationStatus _status = NotificationStatus.Pending;
    private NotificationDeliveryMethod _deliveryMethod = NotificationDeliveryMethod.InApp;

    #endregion

    #region Building Blocks

    /// <summary>
    /// Sets the notification priority
    /// </summary>
    public NotificationBuilder WithPriority(NotificationPriority priority)
    {
        _priority = priority;
        return this;
    }

    /// <summary>
    /// Sets the notification as high priority
    /// </summary>
    public NotificationBuilder AsHighPriority()
    {
        _priority = NotificationPriority.High;
        return this;
    }

    /// <summary>
    /// Sets the notification status
    /// </summary>
    public NotificationBuilder WithStatus(NotificationStatus status)
    {
        _status = status;
        return this;
    }

    /// <summary>
    /// Sets the delivery method
    /// </summary>
    public NotificationBuilder DeliverVia(NotificationDeliveryMethod deliveryMethod)
    {
        _deliveryMethod = deliveryMethod;
        return this;
    }

    /// <summary>
    /// Sets the notification to be delivered via email
    /// </summary>
    public NotificationBuilder DeliverViaEmail()
    {
        _deliveryMethod = NotificationDeliveryMethod.Email;
        return this;
    }

    /// <summary>
    /// Sets the notification to be delivered via SMS
    /// </summary>
    public NotificationBuilder DeliverViaSms()
    {
        _deliveryMethod = NotificationDeliveryMethod.Sms;
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the Notification instance
    /// </summary>
    public Notification Build()
    {
        return Notification.Create(
            Guid.NewGuid(),
            _message,
            type,
            _priority,
            _status,
            userId,
            _deliveryMethod);
    }

    #endregion
}

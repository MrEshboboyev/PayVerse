using PayVerse.Domain.Builders.Notifications;
using PayVerse.Domain.Enums.Notifications;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Notifications;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Notifications;
using PayVerse.Domain.Visitors;

namespace PayVerse.Domain.Entities.Notifications;

public sealed class Notification : AggregateRoot, IAuditableEntity, IVisitable
{
    #region Constructors

    private Notification(
        Guid id,
        NotificationMessage message,
        NotificationType type,
        NotificationPriority priority,
        NotificationStatus status,
        Guid userId,
        NotificationDeliveryMethod deliveryMethod)
        : base(id)
    {
        Message = message;
        Type = type;
        Priority = priority;
        Status = status;
        UserId = userId;
        DeliveryMethod = deliveryMethod;
        IsRead = false;
        IsSent = false;
        SentAt = null;
        ReadAt = null;
        CreatedOnUtc = DateTime.UtcNow;
        ModifiedOnUtc = null;

        RaiseDomainEvent(new NotificationCreatedDomainEvent(
            Guid.NewGuid(),
            Id));
    }

    #endregion

    #region Properties

    public NotificationMessage Message { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationPriority Priority { get; private set; }
    public NotificationStatus Status { get; private set; }
    public Guid UserId { get; private set; }
    public NotificationDeliveryMethod DeliveryMethod { get; private set; }
    public bool IsRead { get; private set; }
    public bool IsSent { get; private set; }
    public DateTime? SentAt { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion

    #region Factory Methods

    public static Notification Create(
        Guid id,
        NotificationMessage message,
        NotificationType type,
        NotificationPriority priority,
        NotificationStatus status,
        Guid userId,
        NotificationDeliveryMethod deliveryMethod)
    {
        return new Notification(id, message, type, priority, status, userId, deliveryMethod);
    }

    #endregion

    #region Own Methods

    public Result MarkAsRead()
    {
        if (IsRead)
        {
            return Result.Failure(DomainErrors.Notification.AlreadyRead(Id));
        }

        IsRead = true;
        ReadAt = DateTime.UtcNow;
        
        RaiseDomainEvent(new NotificationReadDomainEvent(Guid.NewGuid(), Id));

        return Result.Success();
    }

    public Result Send()
    {
        if (IsSent)
        {
            return Result.Failure(
                DomainErrors.Notification.AlreadySent(Id));
        }

        IsSent = true;
        SentAt = DateTime.UtcNow;
        Status = NotificationStatus.Sent;

        RaiseDomainEvent(new NotificationSentDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    public Result ResendNotification()
    {
        if (!IsSent)
        {
            return Result.Failure(
                DomainErrors.Notification.NotSentYet(Id));
        }

        IsSent = false; // Reset to allow re-sending
        SentAt = null;
        Status = NotificationStatus.Pending;

        RaiseDomainEvent(new NotificationResentDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    public Result UpdateMessage(NotificationMessage newMessage)
    {
        if (IsSent)
        {
            return Result.Failure(
                DomainErrors.Notification.CannotModifySentNotification(Id));
        }

        Message = newMessage;
        ModifiedOnUtc = DateTime.UtcNow;

        RaiseDomainEvent(new NotificationUpdatedDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    #endregion

    #region Builders

    // Factory method for the builder
    public static NotificationBuilder CreateBuilder(Guid userId,
                                                    string message,
                                                    NotificationType type)
    {
        return new NotificationBuilder(userId, message, type);
    }

    #endregion

    #region Visitor Pattern Implementation

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    #endregion
}

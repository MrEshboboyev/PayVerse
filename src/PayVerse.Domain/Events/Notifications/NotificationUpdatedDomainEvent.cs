namespace PayVerse.Domain.Events.Notifications;

public sealed record NotificationUpdatedDomainEvent(
    Guid Id,
    Guid NotificationId) : DomainEvent(Id);
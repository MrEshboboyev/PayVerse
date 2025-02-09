namespace PayVerse.Domain.Events.Notifications;

public sealed record NotificationCreatedDomainEvent(
    Guid Id,
    Guid NotificationId) : DomainEvent(Id);
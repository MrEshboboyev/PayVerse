namespace PayVerse.Domain.Events.Notifications;

public sealed record NotificationReadDomainEvent(
    Guid Id,
    Guid NotificationId) : DomainEvent(Id);
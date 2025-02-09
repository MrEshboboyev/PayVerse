namespace PayVerse.Domain.Events.Notifications;

public sealed record NotificationResentDomainEvent(
    Guid Id,
    Guid NotificationId) : DomainEvent(Id);
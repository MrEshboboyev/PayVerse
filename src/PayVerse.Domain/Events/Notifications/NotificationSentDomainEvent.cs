namespace PayVerse.Domain.Events.Notifications;

public sealed record NotificationSentDomainEvent(
    Guid Id,
    Guid NotificationId) : DomainEvent(Id);
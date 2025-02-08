namespace PayVerse.Domain.Events.Users;

public sealed record UserBlockedDomainEvent(
    Guid Id,
    Guid UserId) : DomainEvent(Id);
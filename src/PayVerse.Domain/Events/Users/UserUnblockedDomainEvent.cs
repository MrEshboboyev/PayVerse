namespace PayVerse.Domain.Events.Users;

public sealed record UserUnblockedDomainEvent(
    Guid Id,
    Guid UserId) : DomainEvent(Id);
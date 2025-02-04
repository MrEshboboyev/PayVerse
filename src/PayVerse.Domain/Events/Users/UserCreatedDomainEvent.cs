namespace PayVerse.Domain.Events.Users;

public sealed record UserCreatedDomainEvent(
    Guid Id,
    Guid UserId,
    string Email) : DomainEvent(Id);
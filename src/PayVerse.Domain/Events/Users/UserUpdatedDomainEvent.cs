namespace PayVerse.Domain.Events.Users;

public sealed record UserUpdatedDomainEvent(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName) : DomainEvent(Id);
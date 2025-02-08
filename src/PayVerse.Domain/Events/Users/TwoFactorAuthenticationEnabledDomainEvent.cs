namespace PayVerse.Domain.Events.Users;

public sealed record TwoFactorAuthenticationEnabledDomainEvent(
    Guid Id,
    Guid UserId) : DomainEvent(Id);
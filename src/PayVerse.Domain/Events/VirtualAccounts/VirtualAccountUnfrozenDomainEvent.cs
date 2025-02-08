namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record VirtualAccountUnfrozenDomainEvent(
    Guid Id,
    Guid AccountId) : DomainEvent(Id);
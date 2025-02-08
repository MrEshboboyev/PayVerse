namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record VirtualAccountFrozenDomainEvent(
    Guid Id,
    Guid AccountId) : DomainEvent(Id);

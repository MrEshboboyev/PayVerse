namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record OverdraftLimitSetDomainEvent(
    Guid Id,
    Guid AccountId,
    decimal OverdraftLimit) : DomainEvent(Id);

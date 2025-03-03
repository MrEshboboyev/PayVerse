namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record VirtualAccountDepositedDomainEvent(
    Guid Id,
    Guid AccountId,
    decimal Amount,
    string Description) : DomainEvent(Id);

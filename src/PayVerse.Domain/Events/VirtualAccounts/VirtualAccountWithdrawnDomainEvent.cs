namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record VirtualAccountWithdrawnDomainEvent(
    Guid Id,
    Guid AccountId,
    decimal Amount,
    string Description) : DomainEvent(Id);

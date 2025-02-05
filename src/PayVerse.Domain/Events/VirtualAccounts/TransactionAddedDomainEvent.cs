namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record TransactionAddedDomainEvent(
    Guid Id,
    Guid VirtualAccountId,
    Guid TransactionId) : DomainEvent(Id);
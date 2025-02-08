namespace PayVerse.Domain.Events.VirtualAccounts;

public sealed record FundsTransferredDomainEvent(
    Guid Id,
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount) : DomainEvent(Id);
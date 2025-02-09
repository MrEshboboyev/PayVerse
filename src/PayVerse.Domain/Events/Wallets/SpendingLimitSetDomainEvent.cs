namespace PayVerse.Domain.Events.Wallets;

public sealed record SpendingLimitSetDomainEvent(
    Guid Id,
    Guid WalletId,
    decimal SpendingLimit) : DomainEvent(Id);

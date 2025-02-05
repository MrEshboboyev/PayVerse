namespace PayVerse.Domain.Events.Wallets;

public record WalletTransactionRemovedDomainEvent(
    Guid Id,
    Guid WalletId,
    Guid WalletTransactionId) : DomainEvent(Id);
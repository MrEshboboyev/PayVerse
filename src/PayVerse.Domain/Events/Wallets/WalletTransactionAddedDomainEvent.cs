namespace PayVerse.Domain.Events.Wallets;

public record WalletTransactionAddedDomainEvent(
    Guid Id,
    Guid WalletId,
    Guid WalletTransactionId) : DomainEvent(Id);
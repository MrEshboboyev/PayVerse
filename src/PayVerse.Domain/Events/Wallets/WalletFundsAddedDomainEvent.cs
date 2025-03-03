namespace PayVerse.Domain.Events.Wallets;

public record WalletFundsAddedDomainEvent(
    Guid Id,
    Guid WalletId,
    decimal Amount,
    string Description) : DomainEvent(Id);
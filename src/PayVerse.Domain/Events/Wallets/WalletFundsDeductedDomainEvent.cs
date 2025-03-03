namespace PayVerse.Domain.Events.Wallets;

public record WalletFundsDeductedDomainEvent(
    Guid Id,
    Guid WalletId,
    decimal Amount,
    string Description) : DomainEvent(Id);
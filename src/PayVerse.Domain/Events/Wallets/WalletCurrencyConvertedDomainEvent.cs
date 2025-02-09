namespace PayVerse.Domain.Events.Wallets;

public sealed record WalletCurrencyConvertedDomainEvent(
    Guid Id,
    Guid WalletId,
    string OldCurrencyCode,
    string NewCurrencyCode) : DomainEvent(Id);

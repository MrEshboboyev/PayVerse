namespace PayVerse.Presentation.Contracts.Wallets;

public sealed record CreateWalletRequest(
    decimal Balance,
    string CurrencyCode);
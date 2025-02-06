namespace PayVerse.Presentation.Contracts.Wallets;

public sealed record AddWalletTransactionRequest(
    decimal Amount,
    DateTime Date,
    string Description);
namespace PayVerse.Application.Wallets.Queries.Common.Responses;

public sealed record WalletTransactionResponse(
    Guid WalletTransactionId,
    decimal WalletTransactionAmount,
    DateTime WalletTransactionDate,
    string WalletTransactionDescription);
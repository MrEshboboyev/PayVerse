namespace PayVerse.Application.Wallets.Queries.Common.Responses;

public sealed record WalletWithTransactionsResponse(
    WalletResponse Wallet,
    IReadOnlyList<WalletTransactionResponse> Transactions);
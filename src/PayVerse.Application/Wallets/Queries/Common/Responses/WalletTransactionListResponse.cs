namespace PayVerse.Application.Wallets.Queries.Common.Responses;

public sealed record WalletTransactionListResponse(IReadOnlyCollection<WalletTransactionResponse> Transactions);
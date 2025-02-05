namespace PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

public sealed record TransactionListResponse(IReadOnlyCollection<TransactionResponse> Transactions);
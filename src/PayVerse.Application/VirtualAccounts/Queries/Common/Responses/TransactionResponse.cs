namespace PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

public sealed record TransactionResponse(
    Guid TransactionId,
    decimal Amount,
    string Description);
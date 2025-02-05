namespace PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

public sealed record VirtualAccountResponse(
    Guid VirtualAccountId,
    string AccountNumber,
    string CurrencyCode,
    decimal Balance,
    Guid UserId,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);
namespace PayVerse.Application.Wallets.Queries.Common.Responses;

public sealed record WalletResponse(
    Guid WalletId,
    decimal WalletBalance,
    string CurrencyCode,
    Guid UserId,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);
namespace PayVerse.Application.Wallets.Queries.Common.Responses;

public sealed record WalletListResponse(IReadOnlyCollection<WalletResponse> Wallets);
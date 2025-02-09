using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Queries.GetLoyaltyPointsBalance;

public sealed record GetLoyaltyPointsBalanceQuery(
    Guid WalletId) : IQuery<int>;
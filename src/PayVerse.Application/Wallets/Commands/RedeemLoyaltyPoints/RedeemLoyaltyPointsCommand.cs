using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Wallets.Commands.RedeemLoyaltyPoints;

public sealed record RedeemLoyaltyPointsCommand(
    Guid WalletId,
    int Points) : ICommand;
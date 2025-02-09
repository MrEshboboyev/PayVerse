using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Wallets;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Application.Wallets.Events;

internal sealed class LoyaltyPointsRedeemedDomainEventHandler(
    IWalletRepository walletRepository) : IDomainEventHandler<LoyaltyPointsRedeemedDomainEvent>
{
    public async Task Handle(
        LoyaltyPointsRedeemedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByIdAsync(notification.WalletId, cancellationToken);
        if (wallet is null)
        {
            return;
        }

        Console.WriteLine($"Loyalty points: {notification.PointsRedeemed} redeemed " +
                          $"for wallet [ID: {wallet.Id}]");
    }
}

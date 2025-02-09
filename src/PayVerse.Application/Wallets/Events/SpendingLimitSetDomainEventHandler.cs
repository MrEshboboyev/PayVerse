using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Wallets;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Application.Wallets.Events;

internal sealed class SpendingLimitSetDomainEventHandler(
    IWalletRepository walletRepository) : IDomainEventHandler<SpendingLimitSetDomainEvent>
{
    public async Task Handle(
        SpendingLimitSetDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByIdAsync(notification.WalletId, cancellationToken);
        if (wallet is null)
        {
            return;
        }

        Console.WriteLine($"Spending limit of {notification.SpendingLimit} set for wallet [ID: {wallet.Id}]");
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Wallets;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Application.Wallets.Events;

internal sealed class WalletCurrencyConvertedDomainEventHandler(
    IWalletRepository walletRepository) : IDomainEventHandler<WalletCurrencyConvertedDomainEvent>
{
    public async Task Handle(
        WalletCurrencyConvertedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByIdAsync(notification.WalletId, cancellationToken);
        if (wallet is null)
        {
            return;
        }

        Console.WriteLine($"Wallet [ID : {wallet.Id}] currency converted " +
                          $"from {notification.OldCurrencyCode} to {notification.NewCurrencyCode}");
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Events.Wallets;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Application.Wallets.Events;

internal sealed class WalletCreatedDomainEventHandler(
    IWalletRepository walletRepository) : IDomainEventHandler<WalletCreatedDomainEvent>
{
    public async Task Handle(
        WalletCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByIdAsync(
            notification.Id,
            cancellationToken);

        if (wallet is null)
        {
            return;
        }
        
        Console.WriteLine($"Created wallet with balance : {wallet.Balance.Value}");
    }
}
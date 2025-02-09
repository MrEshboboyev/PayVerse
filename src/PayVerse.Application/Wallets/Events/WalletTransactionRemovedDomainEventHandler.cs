using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.Events.Wallets;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Application.Wallets.Events;

internal sealed class WalletTransactionRemovedDomainEventHandler(
    IWalletRepository walletRepository) : IDomainEventHandler<WalletTransactionRemovedDomainEvent>
{
    public async Task Handle(
        WalletTransactionRemovedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        #region Get this Wallet
        
        var wallet = await walletRepository.GetByIdAsync(
            notification.Id,
            cancellationToken);

        if (wallet is null)
        {
            return;
        }
        
        #endregion
        
        #region Get this transaction from Wallet
        
        var walletTransaction = wallet.GetTransactionById(notification.WalletTransactionId);
        if (walletTransaction is null)
        {
            return;
        }
        
        #endregion

        Console.WriteLine();
        
        Console.WriteLine($"Wallet Transaction removed with amount : {walletTransaction.Amount.Value}" +
                          $" from this wallet [ID : {wallet.Id}]");
    }
}
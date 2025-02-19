using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Infrastructure.Proxies;

public class WalletTransactionProxy(
    IWalletTransaction walletTransaction, 
    Guid walletId) : IWalletTransaction
{
    public void Execute(WalletTransaction transaction)
    {
        if (transaction.WalletId != walletId)
        {
            Console.WriteLine("Unauthorized transaction attempt.");
            return;
        }

        walletTransaction.Execute(transaction);
    }
}

using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Domain.Repositories.Wallets;

public interface IWalletTransactionRepository
{
    Task AddAsync(WalletTransaction walletTransaction, CancellationToken cancellationToken = default);
}
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Persistence.Wallets.Repositories;

public sealed class WalletTransactionRepository(ApplicationDbContext dbContext) : IWalletTransactionRepository
{
    public async Task AddAsync(WalletTransaction walletTransaction, CancellationToken cancellationToken = default)
        => await dbContext.Set<WalletTransaction>().AddAsync(walletTransaction, cancellationToken);
}
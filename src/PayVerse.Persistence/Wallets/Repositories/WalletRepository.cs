using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.Repositories.Wallets;

namespace PayVerse.Persistence.Wallets.Repositories;

public sealed class WalletRepository(ApplicationDbContext dbContext) : IWalletRepository
{
    public async Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<Wallet>().ToListAsync(cancellationToken);

    public async Task<IEnumerable<Wallet>> GetAllByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
        => await dbContext.Set<Wallet>().Where(w => w.UserId == userId).ToListAsync(cancellationToken);

    public async Task<Wallet> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Set<Wallet>().FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public async Task<Wallet> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Set<Wallet>().Include(w => w.Transactions)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
        => await dbContext.Set<Wallet>().AddAsync(wallet, cancellationToken);

    public async Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default)
    {
        dbContext.Set<Wallet>().Update(wallet);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var wallet = await GetByIdAsync(id, cancellationToken);
        if (wallet != null)
        {
            dbContext.Set<Wallet>().Remove(wallet);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
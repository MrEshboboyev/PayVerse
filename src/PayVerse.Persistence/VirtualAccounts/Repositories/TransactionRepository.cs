using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Persistence.VirtualAccounts.Repositories;

public sealed class TransactionRepository(ApplicationDbContext dbContext) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
        => await dbContext.Set<Transaction>().AddAsync(transaction, cancellationToken);

    public async Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        dbContext.Set<Transaction>().Remove(transaction);
        await dbContext.SaveChangesAsync();
    }
}
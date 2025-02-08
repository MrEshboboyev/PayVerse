using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Repositories.VirtualAccounts;

namespace PayVerse.Persistence.VirtualAccounts.Repositories;

public sealed class VirtualAccountRepository(ApplicationDbContext dbContext) : IVirtualAccountRepository
{
    public async Task<IEnumerable<VirtualAccount>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<VirtualAccount>().ToListAsync(cancellationToken);

    public async Task<IEnumerable<VirtualAccount>> GetAllByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
        => await dbContext.Set<VirtualAccount>().Where(va => va.UserId == userId).ToListAsync(cancellationToken);

    public async Task<VirtualAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Set<VirtualAccount>().FirstOrDefaultAsync(va => va.Id == id, cancellationToken);

    public async Task<VirtualAccount> GetByIdWithTransactionsAsync(Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext.Set<VirtualAccount>()
            .Include(va => va.Transactions)
            .FirstOrDefaultAsync(va => va.Id == id, cancellationToken);

    public async Task AddAsync(VirtualAccount virtualAccount, CancellationToken cancellationToken = default)
        => await dbContext.Set<VirtualAccount>().AddAsync(virtualAccount, cancellationToken);

    public async Task UpdateAsync(VirtualAccount virtualAccount, CancellationToken cancellationToken = default)
    {
        dbContext.Set<VirtualAccount>().Update(virtualAccount);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(VirtualAccount virtualAccount, CancellationToken cancellationToken = default)
    {
        dbContext.Set<VirtualAccount>().Remove(virtualAccount);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Transaction>> GetTransactionsByDateAsync(
        Guid accountId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var account = await dbContext.Set<VirtualAccount>()
            .Include(va => va.Transactions)
            .FirstOrDefaultAsync(va => va.Id == accountId, cancellationToken);

        return account?.Transactions.Where(t => t.Date >= startDate && t.Date <= endDate) ?? Enumerable.Empty<Transaction>();
    }
}
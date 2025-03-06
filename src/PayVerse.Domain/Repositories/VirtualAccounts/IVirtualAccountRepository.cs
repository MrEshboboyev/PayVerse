using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Repositories.VirtualAccounts;

public interface IVirtualAccountRepository : IRepository<VirtualAccount>
{
    Task<IEnumerable<VirtualAccount>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VirtualAccount>> GetAllActiveAccountsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VirtualAccount>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<VirtualAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VirtualAccount> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task AddAsync(VirtualAccount virtualAccount, CancellationToken cancellationToken = default);
    Task UpdateAsync(VirtualAccount virtualAccount, CancellationToken cancellationToken = default);
    Task DeleteAsync(VirtualAccount virtualAccount, CancellationToken cancellationToken = default);

    Task<IEnumerable<Transaction>> GetTransactionsByDateAsync(
        Guid accountId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);
}
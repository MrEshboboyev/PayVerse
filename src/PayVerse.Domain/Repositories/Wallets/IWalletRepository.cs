using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Domain.Repositories.Wallets;

public interface IWalletRepository : IRepository<Wallet>
{
    Task<IEnumerable<Wallet>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Wallet>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Wallet> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Wallet> GetByIdWithTransactionsAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default);
    Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
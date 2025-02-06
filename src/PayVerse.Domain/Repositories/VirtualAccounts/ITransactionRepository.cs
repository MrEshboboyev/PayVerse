using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Repositories.VirtualAccounts;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
    Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default);
}
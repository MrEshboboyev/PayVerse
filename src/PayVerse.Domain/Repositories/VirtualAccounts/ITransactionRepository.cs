using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Repositories.VirtualAccounts;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task DeleteAsync(Transaction transaction);
}
using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Observers;

public interface ITransactionObserver
{
    Task OnTransactionProcessedAsync(Transaction transaction);
}
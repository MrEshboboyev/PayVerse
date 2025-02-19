using PayVerse.Application.Collections;
using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Application.Services;

public class TransactionService
{
    public List<Transaction> GetPaginatedTransactions(TransactionList transactions,
                                                      int pageSize,
                                                      int pageNumber)
    {
        var result = new List<Transaction>();
        var iterator = transactions.CreateIterator();
        int itemsProcessed = 0;
        int start = (pageNumber - 1) * pageSize;

        while (iterator.HasNext() && itemsProcessed < pageSize * pageNumber)
        {
            if (itemsProcessed >= start)
            {
                result.Add(iterator.Next());
            }
            else
            {
                iterator.Next(); // Skip items not in the current page
            }
            itemsProcessed++;
        }

        return result;
    }
}
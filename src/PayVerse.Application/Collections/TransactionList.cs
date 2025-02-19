using PayVerse.Domain.Collections;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Iterators;

namespace PayVerse.Application.Collections;

public class TransactionList(IEnumerable<Transaction> transactions) : IAggregate<Transaction>
{
    private readonly List<Transaction> _transactions = [.. transactions];

    public IIterator<Transaction> CreateIterator()
    {
        return new TransactionIterator(_transactions);
    }

    // Additional methods for managing the list
    public void Add(Transaction transaction)
    {
        _transactions.Add(transaction);
    }

    public void Remove(Transaction transaction)
    {
        _transactions.Remove(transaction);
    }
}
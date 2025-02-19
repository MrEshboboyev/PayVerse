using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Iterators;

namespace PayVerse.Application.Collections;

public class TransactionIterator(List<Transaction> transactions) : IIterator<Transaction>
{
    private readonly List<Transaction> _transactions = transactions;
    private int _position = 0;

    public bool HasNext()
    {
        return _position < _transactions.Count;
    }

    public Transaction Next()
    {
        if (!HasNext())
            throw new InvalidOperationException("No more transactions to iterate over.");
        return _transactions[_position++];
    }
}
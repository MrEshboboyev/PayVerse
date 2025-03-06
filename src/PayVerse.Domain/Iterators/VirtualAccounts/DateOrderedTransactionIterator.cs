using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Iterators.VirtualAccounts;

/// <summary>
/// Concrete iterator for transactions ordered by date
/// </summary>
public class DateOrderedTransactionIterator : IIterator<Transaction>
{
    private readonly IReadOnlyCollection<Transaction> _transactions;
    private readonly List<Transaction> _orderedTransactions;
    private int _position = -1;

    public DateOrderedTransactionIterator(IReadOnlyCollection<Transaction> transactions)
    {
        _transactions = transactions;
        _orderedTransactions = [.. _transactions.OrderBy(t => t.Date)];
    }

    public bool HasNext()
    {
        return _position < _orderedTransactions.Count - 1;
    }

    public Transaction Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No more transactions to iterate");
        }

        return _orderedTransactions[++_position];
    }

    public void Reset()
    {
        _position = -1;
    }

    public Transaction Current()
    {
        if (_position < 0 || _position >= _orderedTransactions.Count)
        {
            throw new InvalidOperationException("Invalid iterator position");
        }

        return _orderedTransactions[_position];
    }
}
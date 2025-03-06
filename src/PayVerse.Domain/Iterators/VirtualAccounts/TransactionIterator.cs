using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Iterators.VirtualAccounts;


/// <summary>
/// Concrete iterator for VirtualAccount transactions
/// </summary>
public class TransactionIterator(
    IReadOnlyCollection<Transaction> transactions) : IIterator<Transaction>
{
    private int _position = -1;
    private readonly List<Transaction> _transactionsList = [.. transactions];

    public bool HasNext()
    {
        return _position < _transactionsList.Count - 1;
    }

    public Transaction Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No more transactions to iterate");
        }

        return _transactionsList[++_position];
    }

    public void Reset()
    {
        _position = -1;
    }

    public Transaction Current()
    {
        if (_position < 0 || _position >= _transactionsList.Count)
        {
            throw new InvalidOperationException("Invalid iterator position");
        }

        return _transactionsList[_position];
    }
}

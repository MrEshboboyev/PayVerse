using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Domain.Iterators.Wallets;


/// <summary>
/// Iterator for wallet transactions
/// </summary>
public class WalletTransactionIterator(
    IReadOnlyCollection<WalletTransaction> transactions) : IIterator<WalletTransaction>
{
    private int _position = -1;
    private readonly List<WalletTransaction> _transactionsList = [.. transactions];

    public bool HasNext()
    {
        return _position < _transactionsList.Count - 1;
    }

    public WalletTransaction Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No more wallet transactions to iterate");
        }

        return _transactionsList[++_position];
    }

    public void Reset()
    {
        _position = -1;
    }

    public WalletTransaction Current()
    {
        if (_position < 0 || _position >= _transactionsList.Count)
        {
            throw new InvalidOperationException("Invalid iterator position");
        }

        return _transactionsList[_position];
    }
}
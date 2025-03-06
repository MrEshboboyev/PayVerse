using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Iterators.VirtualAccounts;

/// <summary>
/// Concrete iterator for filtered transactions based on amount
/// </summary>
public class AmountFilteredTransactionIterator : IIterator<Transaction>
{
    private readonly IReadOnlyCollection<Transaction> _transactions;
    private readonly decimal _minimumAmount;
    private readonly List<Transaction> _filteredTransactions;
    private int _position = -1;

    public AmountFilteredTransactionIterator(
        IReadOnlyCollection<Transaction> transactions,
        decimal minimumAmount)
    {
        _transactions = transactions;
        _minimumAmount = minimumAmount;
        _filteredTransactions = [.. _transactions.Where(t => t.Amount.Value >= _minimumAmount)];
    }

    public bool HasNext()
    {
        return _position < _filteredTransactions.Count - 1;
    }

    public Transaction Next()
    {
        if (!HasNext())
        {
            throw new InvalidOperationException("No more transactions to iterate");
        }

        return _filteredTransactions[++_position];
    }

    public void Reset()
    {
        _position = -1;
    }

    public Transaction Current()
    {
        if (_position < 0 || _position >= _filteredTransactions.Count)
        {
            throw new InvalidOperationException("Invalid iterator position");
        }

        return _filteredTransactions[_position];
    }
}

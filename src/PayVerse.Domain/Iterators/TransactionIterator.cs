using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Iterators;

public class TransactionIterator(List<Transaction> transactions) : IIterator<Transaction>
{
    private readonly List<Transaction> _transactions = transactions;
    private int _position = 0;

    public bool HasNext() => _position < _transactions.Count;
    public Transaction Next() => _transactions[_position++];
}

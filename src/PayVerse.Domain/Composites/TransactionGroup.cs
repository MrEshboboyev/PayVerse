namespace PayVerse.Domain.Composites;

public class TransactionGroup : ITransactionComponent
{
    private readonly List<ITransactionComponent> _transactions = [];

    public void AddTransaction(ITransactionComponent transaction)
    {
        _transactions.Add(transaction);
    }

    public void Process()
    {
        Console.WriteLine($"Processing batch transaction...");
        foreach (var transaction in _transactions)
        {
            transaction.Process();
        }
    }
}

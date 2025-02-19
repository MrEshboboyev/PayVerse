namespace PayVerse.Domain.Composites;

public class TransactionLeaf(string transactionId, decimal amount) : ITransactionComponent
{
    public void Process()
    {
        Console.WriteLine($"Processing single transaction {transactionId} of ${amount}");
    }
}

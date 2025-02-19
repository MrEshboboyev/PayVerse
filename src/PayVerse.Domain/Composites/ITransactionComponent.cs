namespace PayVerse.Domain.Composites;

// ✅ Benefits:
// Enables batch processing of transactions.
// Reduces overhead when processing large numbers of transactions.

public interface ITransactionComponent
{
    void Process();
}

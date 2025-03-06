using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.Iterators;

namespace PayVerse.Application.Services;


/// <summary>
/// Example service that demonstrates the use of iterators for financial transactions analysis
/// </summary>
public class TransactionAnalysisService
{
    /// <summary>
    /// Calculates the total amount of transactions in a virtual account
    /// </summary>
    /// <param name="account">The virtual account to analyze</param>
    /// <returns>Total amount of all transactions</returns>
    public decimal CalculateTotalTransactionAmount(VirtualAccount account)
    {
        decimal total = 0;
        IIterator<Transaction> iterator = account.CreateIterator();

        while (iterator.HasNext())
        {
            var transaction = iterator.Next();
            total += transaction.Amount.Value;
        }

        return total;
    }

    /// <summary>
    /// Calculates the total amount of large transactions in a virtual account
    /// </summary>
    /// <param name="account">The virtual account to analyze</param>
    /// <param name="threshold">The minimum amount to be considered a large transaction</param>
    /// <returns>Total amount of large transactions</returns>
    public decimal CalculateLargeTransactionsAmount(VirtualAccount account, decimal threshold)
    {
        decimal total = 0;
        IIterator<Transaction> iterator = account.CreateAmountFilteredIterator(threshold);

        while (iterator.HasNext())
        {
            var transaction = iterator.Next();
            total += transaction.Amount.Value;
        }

        return total;
    }

    /// <summary>
    /// Gets transactions ordered by date and returns the most recent ones
    /// </summary>
    /// <param name="account">The virtual account</param>
    /// <param name="count">Number of recent transactions to return</param>
    /// <returns>List of most recent transactions</returns>
    public List<TransactionSummary> GetRecentTransactions(VirtualAccount account, int count)
    {
        var result = new List<TransactionSummary>();
        IIterator<Transaction> iterator = account.CreateDateOrderedIterator();

        // Move iterator to the end
        while (iterator.HasNext())
        {
            iterator.Next();
        }

        // Navigate backward (not supported by our simple iterator, so we'll reimplement)
        var orderedTransactions = account.Transactions
            .OrderByDescending(t => t.Date)
            .Take(count)
            .ToList();

        foreach (var transaction in orderedTransactions)
        {
            result.Add(new TransactionSummary(
                transaction.Id,
                transaction.Amount.Value,
                transaction.Date,
                transaction.Description));
        }

        return result;
    }

    /// <summary>
    /// Compares transactions between a wallet and a virtual account
    /// </summary>
    /// <param name="wallet">The wallet</param>
    /// <param name="account">The virtual account</param>
    /// <returns>A comparison report</returns>
    public TransactionComparisonReport CompareWalletAndAccountTransactions(
        Wallet wallet,
        VirtualAccount account)
    {
        decimal walletTotal = 0;
        decimal accountTotal = 0;

        IIterator<WalletTransaction> walletIterator = wallet.CreateIterator();
        IIterator<Transaction> accountIterator = account.CreateIterator();

        while (walletIterator.HasNext())
        {
            var transaction = walletIterator.Next();
            walletTotal += transaction.Amount.Value;
        }

        while (accountIterator.HasNext())
        {
            var transaction = accountIterator.Next();
            accountTotal += transaction.Amount.Value;
        }

        return new TransactionComparisonReport(
            wallet.Id,
            account.Id,
            walletTotal,
            accountTotal,
            walletTotal - accountTotal);
    }
}

/// <summary>
/// Data transfer object for transaction summary
/// </summary>
public class TransactionSummary(Guid id, decimal amount, DateTime date, string description)
{
    public Guid Id { get; } = id;
    public decimal Amount { get; } = amount;
    public DateTime Date { get; } = date;
    public string Description { get; } = description;
}

/// <summary>
/// Data transfer object for transaction comparison
/// </summary>
public class TransactionComparisonReport(
    Guid walletId,
    Guid accountId,
    decimal walletTotal,
    decimal accountTotal,
    decimal difference)
{
    public Guid WalletId { get; } = walletId;
    public Guid AccountId { get; } = accountId;
    public decimal WalletTotal { get; } = walletTotal;
    public decimal AccountTotal { get; } = accountTotal;
    public decimal Difference { get; } = difference;
}
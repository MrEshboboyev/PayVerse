using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Entities.Wallets;

/// <summary>
/// Represents a wallet transaction.
/// </summary>
public sealed class WalletTransaction : Entity
{
    #region Constructors
    
    internal WalletTransaction(
        Guid id,
        decimal amount,
        DateTime date,
        string description) : base(id)
    {
        Amount = amount;
        Date = date;
        Description = description;
    }
    
    #endregion
    
    #region Properties

    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    
    #endregion
}
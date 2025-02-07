using PayVerse.Domain.Primitives;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Entities.Wallets;

/// <summary>
/// Represents a wallet transaction.
/// </summary>
public sealed class WalletTransaction : Entity
{
    #region Constructors
    
    internal WalletTransaction(
        Guid id,
        Guid walletId,
        Amount amount,
        DateTime date,
        string description) : base(id)
    {
        WalletId = walletId;
        Amount = amount;
        Date = date;
        Description = description;
    }
    
    #endregion
    
    #region Properties

    public Guid WalletId { get; private set; }
    public Amount Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    
    #endregion
}
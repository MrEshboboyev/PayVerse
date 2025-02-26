using PayVerse.Domain.Primitives;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Entities.Wallets;

/// <summary>
/// Represents a wallet transaction with Prototype pattern implementation
/// </summary>
public sealed class WalletTransaction : Entity, IPrototype<WalletTransaction>
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

    // Copy constructor for Prototype pattern
    private WalletTransaction(WalletTransaction source) : base(source.Id)
    {
        WalletId = source.WalletId;
        Amount = source.Amount;
        Date = source.Date;
        Description = source.Description;
    }

    #endregion

    #region Properties

    public Guid WalletId { get; private set; }
    public Amount Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }

    #endregion

    #region Prototype Methods
    public WalletTransaction ShallowCopy()
    {
        return new WalletTransaction(
            Id,
            WalletId,
            Amount,
            Date,
            Description);
    }

    public WalletTransaction DeepCopy()
    {
        return new WalletTransaction(this);
    }
    #endregion
}
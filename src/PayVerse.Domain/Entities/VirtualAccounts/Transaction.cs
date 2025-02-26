using PayVerse.Domain.Primitives;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Entities.VirtualAccounts;

/// <summary>
/// Represents a financial transaction with Prototype pattern implementation
/// </summary>
public sealed class Transaction : Entity, IPrototype<Transaction>
{
    #region Constructors
    
    internal Transaction(
        Guid id,
        Guid virtualAccountId,
        Amount amount,
        DateTime date,
        string description) : base(id)
    {
        VirtualAccountId = virtualAccountId;
        Amount = amount;
        Date = date;
        Description = description;
    }

    // Copy constructor for Prototype pattern
    private Transaction(Transaction source) : base(source.Id)
    {
        VirtualAccountId = source.VirtualAccountId;
        Amount = source.Amount;
        Date = source.Date;
        Description = source.Description;
    }

    #endregion

    #region Properties

    public Guid VirtualAccountId { get; set; }
    public Amount Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }

    #endregion

    #region Prototype Methods
    public Transaction ShallowCopy()
    {
        return new Transaction(
            Id,
            VirtualAccountId,
            Amount,
            Date,
            Description);
    }

    public Transaction DeepCopy()
    {
        return new Transaction(this);
    }
    #endregion
}
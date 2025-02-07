using PayVerse.Domain.Primitives;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Entities.VirtualAccounts;

/// <summary>
/// Represents a financial transaction.
/// </summary>
public sealed class Transaction : Entity
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
    
    #endregion
    
    #region Properties

    public Guid VirtualAccountId { get; set; }
    public Amount Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    
    #endregion
}
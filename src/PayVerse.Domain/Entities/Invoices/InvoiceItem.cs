using PayVerse.Domain.Primitives;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Entities.Invoices;

/// <summary>
/// Represents an invoice in the system with Prototype pattern implementation
/// </summary>
public sealed class InvoiceItem : Entity, IPrototype<InvoiceItem>
{
    #region Constructor
    
    internal InvoiceItem(
        Guid id,
        Guid invoiceId, 
        string description,
        Amount amount) : base(id)
    {
        InvoiceId = invoiceId;
        Description = description;
        Amount = amount;
    }

    // Copy constructor for Prototype pattern
    private InvoiceItem(InvoiceItem source) : base(source.Id)
    {
        InvoiceId = source.InvoiceId;
        Description = source.Description;
        Amount = source.Amount;
    }

    #endregion

    #region Properties

    public Guid InvoiceId { get; private set; }
    public string Description { get; private set; }
    public Amount Amount { get; private set; }

    #endregion

    #region Prototype Methods
    public InvoiceItem ShallowCopy()
    {
        return new InvoiceItem(
            Id,
            InvoiceId,
            Description,
            Amount);
    }

    public InvoiceItem DeepCopy()
    {
        return new InvoiceItem(this);
    }
    #endregion
}
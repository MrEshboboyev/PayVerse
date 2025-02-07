using PayVerse.Domain.Primitives;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Domain.Entities.Invoices;

/// <summary>
/// Represents an item in an invoice.
/// </summary>
public sealed class InvoiceItem : Entity
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
    
    #endregion

    #region Properties

    public Guid InvoiceId { get; private set; }
    public string Description { get; private set; }
    public Amount Amount { get; private set; }
    
    #endregion
}
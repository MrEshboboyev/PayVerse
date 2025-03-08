using PayVerse.Domain.Builders.Invoices;
using PayVerse.Domain.Enums.Invoices;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Invoices;
using PayVerse.Domain.Visitors;

namespace PayVerse.Domain.Entities.Invoices;

/// <summary>
/// Represents an invoice in the system.
/// </summary>
public sealed class Invoice : PrototypeAggregateRoot, IAuditableEntity, IVisitable
{
    #region Private Fields

    private readonly List<InvoiceItem> _items = [];

    #endregion

    #region Constructors

    private Invoice(
        Guid id,
        InvoiceNumber invoiceNumber,
        InvoiceDate invoiceDate,
        Amount totalAmount,
        Guid userId) : base(id)
    {
        Status = InvoiceStatus.Draft;
        InvoiceNumber = invoiceNumber;
        InvoiceDate = invoiceDate;
        TotalAmount = totalAmount;
        UserId = userId;

        #region Domain Events

        RaiseDomainEvent(new InvoiceCreatedDomainEvent(
            Guid.NewGuid(),
            id));

        #endregion
    }

    // Copy constructor for Prototype pattern
    private Invoice(Invoice source) : base(source.Id)
    {
        Status = source.Status;
        RecurringFrequencyInMonths = source.RecurringFrequencyInMonths;
        InvoiceNumber = source.InvoiceNumber;
        InvoiceDate = source.InvoiceDate;
        TotalAmount = source.TotalAmount;
        UserId = source.UserId;
        CreatedOnUtc = source.CreatedOnUtc;
        ModifiedOnUtc = source.ModifiedOnUtc;

        // Copy items for deep copy
        foreach (var item in source._items)
        {
            _items.Add(item.DeepCopy() as InvoiceItem);
        }
    }

    #endregion

    #region Properties

    public InvoiceStatus Status { get; private set; }
    public int? RecurringFrequencyInMonths { get; private set; }
    public bool IsFinalized { get; private set; }
    public DateTime FinalizedDate { get; private set; }
    public InvoiceNumber InvoiceNumber { get; private set; }
    public InvoiceDate InvoiceDate { get; private set; }
    public Amount TotalAmount { get; private set; }
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<InvoiceItem> Items => _items.AsReadOnly();
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion

    #region Factory Methods

    public static Invoice Create(
        Guid id,
        InvoiceNumber number,
        InvoiceDate date,
        Amount amount,
        Guid userId)
    {
        return new Invoice(id, number, date, amount, userId);
    }

    // Factory method to create from a prototype
    public static Invoice CreateFromPrototype(Invoice prototype)
    {
        return prototype.DeepCopy() as Invoice;
    }

    // Factory method to create a recurring invoice from a prototype
    public static Invoice CreateRecurringFromPrototype(Invoice prototype,
                                                       int frequencyInMonths)
    {
        var newInvoice = prototype.DeepCopy() as Invoice;
        newInvoice.SetRecurringFrequency(frequencyInMonths);
        newInvoice.SetInvoiceDate(InvoiceDate.Create(DateTime.UtcNow.AddMonths(frequencyInMonths)).Value);
        return newInvoice;
    }

    #endregion

    #region Prototype Overrides

    public override PrototypeAggregateRoot ShallowCopy()
    {
        return new Invoice(
            Id,
            InvoiceNumber,
            InvoiceDate,
            TotalAmount,
            UserId);
    }

    public override PrototypeAggregateRoot DeepCopy()
    {
        return new Invoice(this);
    }

    #endregion

    #region Builders

    // Factory method for the builder
    public static InvoiceBuilder CreateBuilder(Guid userId)
    {
        return new InvoiceBuilder(userId);
    }

    #endregion

    #region Own methods

    #region Prototype related

    public Result SetRecurringFrequency(int frequencyInMonths)
    {
        if (frequencyInMonths <= 0)
        {
            throw new ArgumentException("Recurring frequency must be a positive value"); // write a domain error
        }

        RecurringFrequencyInMonths = frequencyInMonths;

        return Result.Success();
    }

    public Result SetInvoiceDate(InvoiceDate newDate)
    {
        InvoiceDate = newDate;

        return Result.Success();
    }

    #endregion

    /// <summary>
    /// Cancels the invoice with a given reason.
    /// </summary>
    /// <param name="reason">The reason for cancellation.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result Cancel(string reason)
    {
        if (IsFinalized)
        {
            return Result.Failure(
                DomainErrors.Invoice.CannotCancelFinalizedInvoice(Id));
        }

        Status = InvoiceStatus.Cancelled;

        RaiseDomainEvent(new InvoiceCancelledDomainEvent(
            Guid.NewGuid(),
            Id, 
            reason));

        return Result.Success();
    }

    /// <summary>
    /// Finalizes the invoice.
    /// </summary>
    /// <returns>Result indicating success or failure.</returns>
    public Result Finalize()
    {
        if (IsFinalized)
        {
            return Result.Failure(DomainErrors.Invoice.AlreadyFinalized(Id));
        }

        Status = InvoiceStatus.Finalized;
        FinalizedDate = DateTime.UtcNow;

        RaiseDomainEvent(new InvoiceFinalizedDomainEvent(
            Guid.NewGuid(),
            Id));

        return Result.Success();
    }

    #endregion

    #region Item related Methods

    public InvoiceItem GetItemById(Guid id) => _items.FirstOrDefault(i => i.Id == id);

    public Result<InvoiceItem> AddItem(
        string description,
        Amount amount)
    {
        #region Create Invoice Item
        
        var invoiceItem = new InvoiceItem(
            Guid.NewGuid(),
            Id,
            description,
            amount);
        
        #endregion
        
        #region Add Invoice Item
        
        _items.Add(invoiceItem);
        
        #endregion
        
        #region Domain Events
        
        RaiseDomainEvent(new InvoiceItemAddedDomainEvent(
            Guid.NewGuid(),
            Id,
            invoiceItem.Id));
        
        #endregion
        
        return Result.Success(invoiceItem);
    }

    public Result RemoveItem(Guid itemId)
    {
        #region Checking item exists
        
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
        {
            return Result.Failure(
                DomainErrors.Invoice.ItemNotFound(itemId));
        }
        
        #endregion
        
        #region Remove Invoice Item
        
        _items.Remove(item);  
        
        #endregion
        
        #region Domain Events
        
        RaiseDomainEvent(new InvoiceItemRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            item.Amount.Value));
        
        #endregion
        
        return Result.Success();
    } 

    #endregion
    
    #region Status related Methods
    
    public Result MarkAsPaid()
    {
        Status = InvoiceStatus.Paid;
        
        RaiseDomainEvent(new InvoicePaidDomainEvent(
            Guid.NewGuid(),
            Id));
        
        return Result.Success();
    }

    public Result MarkAsOverdue()
    {
        Status = InvoiceStatus.Overdue;
        
        RaiseDomainEvent(new InvoiceOverdueDomainEvent(
            Guid.NewGuid(),
            Id));
        
        return Result.Success();
    }

    public Result SetRecurring(int frequencyInMonths)
    {
        Status = InvoiceStatus.Recurring;
        
        RecurringFrequencyInMonths = frequencyInMonths;
        
        RaiseDomainEvent(new RecurringInvoiceCreatedDomainEvent(
            Guid.NewGuid(),
            Id,
            frequencyInMonths));
        
        return Result.Success();
    }
    
    #endregion
    
    #region Discount related Methods
    
    public Result ApplyDiscount(Amount discountAmount)
    {
        TotalAmount = Amount.Create(TotalAmount.Value - discountAmount.Value).Value;
        
        RaiseDomainEvent(new InvoiceDiscountAppliedDomainEvent(
            Guid.NewGuid(),
            Id, 
            discountAmount.Value));
        
        return Result.Success();
    }
    
    #endregion
    
    #region Tax related Methods

    public Result AddTax(Amount taxAmount)
    {
        TotalAmount = Amount.Create(TotalAmount.Value + taxAmount.Value).Value;
        
        RaiseDomainEvent(new InvoiceTaxAddedDomainEvent(
            Guid.NewGuid(),
            Id,
            taxAmount.Value));
        
        return Result.Success();
    }

    #endregion

    #region Visitor Pattern Implementation

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    #endregion
}
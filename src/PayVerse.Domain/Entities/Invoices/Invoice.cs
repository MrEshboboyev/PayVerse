using PayVerse.Domain.Enums.Invoices;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Invoices;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Invoices;

namespace PayVerse.Domain.Entities.Invoices;

/// <summary>
/// Represents an invoice in the system.
/// </summary>
public sealed class Invoice : AggregateRoot, IAuditableEntity
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

    #endregion

    #region Properties

    public InvoiceStatus Status { get; private set; }
    public int RecurringFrequencyInMonths { get; private set; }
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
}
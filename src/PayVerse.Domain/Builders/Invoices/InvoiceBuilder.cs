using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Invoices;

namespace PayVerse.Domain.Builders.Invoices;


// ✅ Benefits:
// Makes invoice creation clear and readable(instead of long constructor parameters).
// Allows adding optional features(e.g., discount, tax, fees).
public class InvoiceBuilder(
    Guid userId)
{
    private readonly Invoice _invoice = Invoice.Create(
        Guid.NewGuid(), 
        InvoiceNumber.Generate(),
        InvoiceDate.Create(DateTime.UtcNow).Value,
        Amount.Create(0).Value,
        userId);

    public InvoiceBuilder AddItem(string description, Amount amount)
    {
        _invoice.AddItem(description, amount);
        return this;
    }

    //public InvoiceBuilder ApplyDiscount(decimal percentage)
    //{
    //    _invoice.ApplyDiscount(percentage);
    //    return this;
    //}

    public Invoice Build()
    {
        return _invoice;
    }
}

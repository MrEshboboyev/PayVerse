using PayVerse.Domain.Entities.Invoices;

namespace PayVerse.Domain.Visitors;

public class TaxVisitor : IInvoiceVisitor
{
    public void Visit(Invoice invoice)
    {
        decimal taxRate = 0.10m; // Default tax rate (10%), could be configurable or fetched from domain logic
        decimal taxAmount = invoice.TotalAmount.Value * taxRate;

        Console.WriteLine($"💰 Calculated tax on Invoice {invoice.Id}: ${taxAmount} (Rate: {taxRate * 100}%)");
        // Here you could also update the invoice with tax information or raise a domain event
    }
}

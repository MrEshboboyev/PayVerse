using PayVerse.Domain.Entities.Invoices;

namespace PayVerse.Domain.Visitors;

// ✅ Allows applying new operations to invoices without modifying them.

public interface IInvoiceVisitor
{
    void Visit(Invoice invoice);
}

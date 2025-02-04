using PayVerse.Domain.Entities.Invoices;

namespace PayVerse.Domain.Repositories.Invoices;

public interface IInvoiceItemRepository
{
    Task AddAsync(InvoiceItem invoiceItem, CancellationToken cancellationToken = default);
}
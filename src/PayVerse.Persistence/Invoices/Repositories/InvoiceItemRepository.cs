using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Persistence.Invoices.Repositories;

public sealed class InvoiceItemRepository(ApplicationDbContext dbContext) : IInvoiceItemRepository
{
    public async Task AddAsync(InvoiceItem invoiceItem, CancellationToken cancellationToken = default)
        => await dbContext
            .Set<InvoiceItem>()
            .AddAsync(invoiceItem, cancellationToken);
}
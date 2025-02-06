using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Repositories.Invoices;

namespace PayVerse.Persistence.Invoices.Repositories;

public sealed class InvoiceRepository(ApplicationDbContext dbContext) : IInvoiceRepository
{
    public async Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Invoice>()
            .ToListAsync(cancellationToken);

    public async Task<Invoice> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Invoice>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Invoice> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Invoice>()
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Invoice>()
            .FirstOrDefaultAsync(x => x.InvoiceNumber.Value == invoiceNumber, cancellationToken);

    public async Task<IEnumerable<Invoice>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Invoice>()
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
        => await dbContext
            .Set<Invoice>()
            .AddAsync(invoice, cancellationToken);

    public async Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default)
        => dbContext
            .Set<Invoice>()
            .Update(invoice);

    public async Task DeleteAsync(Invoice invoice, CancellationToken cancellationToken = default)
        => dbContext
            .Set<Invoice>()
            .Remove(invoice);
}
using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Enums.Invoices;
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
    
    public async Task<IEnumerable<Invoice>> GetOverdueAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Invoice>()
            .Where(x => x.Status == InvoiceStatus.Overdue)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Invoice>> GetByStatusAsync(
        InvoiceStatus status,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Invoice>()
            .Where(x => x.Status == status)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Invoice>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Invoice>()
            .Where(x => 
                x.InvoiceDate.Value >= startDate 
                && x.InvoiceDate.Value <= endDate)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<decimal> GetTotalRevenueByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<Invoice>()
            .Where(x =>
                x.UserId == userId 
                && x.Status == InvoiceStatus.Paid)
            .SumAsync(x => x.TotalAmount.Value, cancellationToken);
    }
}
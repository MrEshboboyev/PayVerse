using PayVerse.Domain.Entities.Invoices;
using PayVerse.Domain.Enums.Invoices;

namespace PayVerse.Domain.Repositories.Invoices;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default); // for Admins
    
    Task<Invoice> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Invoice> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task DeleteAsync(Invoice invoice, CancellationToken cancellationToken = default);

    Task<IEnumerable<Invoice>> GetOverdueAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Invoice>> GetByStatusAsync(
        InvoiceStatus status,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Invoice>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<decimal> GetTotalRevenueByUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
using PayVerse.Domain.Entities.Invoices;

namespace PayVerse.Domain.Repositories.Invoices;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default); // for Admins
    
    Task<Invoice> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Invoice>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task DeleteAsync(Invoice invoice, CancellationToken cancellationToken = default);
}
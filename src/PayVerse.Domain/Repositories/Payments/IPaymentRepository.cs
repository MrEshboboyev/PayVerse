using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.Repositories.Payments;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
    
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Payment payment, CancellationToken cancellationToken = default);
}
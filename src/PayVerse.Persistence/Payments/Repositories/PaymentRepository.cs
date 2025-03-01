using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories.Payments;

namespace PayVerse.Persistence.Payments.Repositories;

public sealed class PaymentRepository(ApplicationDbContext dbContext) : IPaymentRepository
{
    public async Task<Payment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Set<Payment>()
            .FirstOrDefaultAsync(payment => payment.Id == id, cancellationToken);

    public async Task<IEnumerable<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<Payment>()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Payment>> GetAllByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
        => await dbContext.Set<Payment>()
            .Where(payment => payment.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status,
        CancellationToken cancellationToken = default)
        => await dbContext.Set<Payment>()
            .Where(payment => payment.Status == status)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
        => await dbContext.Set<Payment>().AddAsync(payment, cancellationToken);

    public async Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        dbContext.Set<Payment>().Update(payment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        dbContext.Set<Payment>().Remove(payment);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Payment>()
            .Where(payment => payment.CreatedOnUtc >= startDate && payment.CreatedOnUtc <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetPaymentsForPeriodAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

        return await dbContext.Set<Payment>()
            .Where(payment => payment.CreatedOnUtc >= startDateTime && payment.CreatedOnUtc <= endDateTime)
            .ToListAsync(cancellationToken);
    }
}
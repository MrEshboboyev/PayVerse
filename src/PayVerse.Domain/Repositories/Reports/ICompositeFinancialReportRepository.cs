using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Domain.Repositories.Reports;

public interface ICompositeFinancialReportRepository
{
    Task<IEnumerable<CompositeFinancialReport>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CompositeFinancialReport> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompositeFinancialReport>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompositeFinancialReport>> GetByPeriodAsync(ReportPeriod period, CancellationToken cancellationToken = default);
    Task AddAsync(CompositeFinancialReport report, CancellationToken cancellationToken = default);
    Task UpdateAsync(CompositeFinancialReport report, CancellationToken cancellationToken = default);
}

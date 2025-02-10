using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Domain.Repositories.Reports;

public interface IFinancialReportRepository
{
    Task<IEnumerable<FinancialReport>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FinancialReport> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancialReport>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinancialReport>> GetByPeriodAsync(ReportPeriod period, CancellationToken cancellationToken = default);
    Task AddAsync(FinancialReport report, CancellationToken cancellationToken = default);
    Task UpdateAsync(FinancialReport report, CancellationToken cancellationToken = default);
}

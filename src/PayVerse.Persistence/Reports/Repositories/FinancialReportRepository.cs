using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Persistence.Reports.Repositories;

public sealed class FinancialReportRepository(ApplicationDbContext dbContext) : IFinancialReportRepository
{
    public async Task<IEnumerable<FinancialReport>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<FinancialReport>()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<FinancialReport> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<FinancialReport>()
            .FirstOrDefaultAsync(report => report.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<FinancialReport>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<FinancialReport>()
            .Where(report => report.GeneratedBy == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<FinancialReport>> GetByPeriodAsync(
        ReportPeriod period,
        CancellationToken cancellationToken = default)
    {
        var startDate = period.StartDate.ToDateTime(new TimeOnly());
        var endDate = period.EndDate.ToDateTime(new TimeOnly());

        return await dbContext.Set<FinancialReport>()
            .Where(report => report.GeneratedAt >= startDate
                             && report.GeneratedAt <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        FinancialReport report,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Set<FinancialReport>().AddAsync(report, cancellationToken);
    }

    public async Task UpdateAsync(
        FinancialReport report,
        CancellationToken cancellationToken = default)
    {
        dbContext.Set<FinancialReport>().Update(report);
    }
}

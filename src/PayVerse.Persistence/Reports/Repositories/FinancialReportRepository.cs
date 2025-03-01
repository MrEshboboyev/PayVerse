using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Repositories.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Persistence.Reports.Repositories;

public sealed class CompositeFinancialReportRepository(ApplicationDbContext dbContext) : ICompositeFinancialReportRepository
{
    public async Task<IEnumerable<CompositeFinancialReport>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<CompositeFinancialReport>()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<CompositeFinancialReport> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<CompositeFinancialReport>()
            .FirstOrDefaultAsync(report => report.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<CompositeFinancialReport>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<CompositeFinancialReport>()
            .Where(report => report.GeneratedBy == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CompositeFinancialReport>> GetByPeriodAsync(
        ReportPeriod period,
        CancellationToken cancellationToken = default)
    {
        var startDate = period.StartDate.ToDateTime(new TimeOnly());
        var endDate = period.EndDate.ToDateTime(new TimeOnly());

        return await dbContext.Set<CompositeFinancialReport>()
            .Where(report => report.GeneratedAt >= startDate
                             && report.GeneratedAt <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        CompositeFinancialReport report,
        CancellationToken cancellationToken = default)
    {
        await dbContext.Set<CompositeFinancialReport>().AddAsync(report, cancellationToken);
    }

    public async Task UpdateAsync(
        CompositeFinancialReport report,
        CancellationToken cancellationToken = default)
    {
        dbContext.Set<CompositeFinancialReport>().Update(report);
    }
}

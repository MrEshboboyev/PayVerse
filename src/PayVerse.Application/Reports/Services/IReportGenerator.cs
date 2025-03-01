using PayVerse.Domain.Entities.Reports;

namespace PayVerse.Application.Reports.Services;

public interface IReportGenerator
{
    Task<string> GenerateAsync(CompositeFinancialReport report, CancellationToken cancellationToken = default);
}
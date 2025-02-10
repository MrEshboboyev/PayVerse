using PayVerse.Domain.Entities.Reports;

namespace PayVerse.Application.Reports.Services;

public interface IReportGenerator
{
    Task<string> GenerateAsync(FinancialReport report, CancellationToken cancellationToken);
}
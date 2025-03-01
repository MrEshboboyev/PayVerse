using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Interfaces;

// ✅ Handles multiple reporting operations.
public interface ICompositeFinancialReportService
{
    Task<Result<CompositeFinancialReport>> GenerateReportAsync(Guid userId,
                                                               ReportTitle title,
                                                               ReportPeriod period,
                                                               ReportType type,
                                                               FileType fileType);
    Task<Result> SaveReportAsync(CompositeFinancialReport report, string filePath);
    Task<Result> SendReportByEmailAsync(CompositeFinancialReport report, string email);
}
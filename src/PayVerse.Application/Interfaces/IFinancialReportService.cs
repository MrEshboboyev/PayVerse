using PayVerse.Domain.Entities.Reports;
using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Interfaces;

// ✅ Handles multiple reporting operations.
public interface IFinancialReportService
{
    Task<Result<FinancialReport>> GenerateReportAsync(Guid userId, ReportPeriod period, ReportType type, FileType fileType);
    Task<Result> SaveReportAsync(FinancialReport report, string filePath);
    Task<Result> SendReportByEmailAsync(FinancialReport report, string email);
}
using PayVerse.Domain.Enums.Reports;

namespace PayVerse.Application.Reports.Queries.Common.Responses;

public sealed record CompositeFinancialReportResponse(
    ReportPeriodResponse ReportPeriod,
    ReportType ReportType,
    ReportStatus ReportStatus,
    string FilePath,
    Guid GeneratedBy,
    DateTime GeneratedAt,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);
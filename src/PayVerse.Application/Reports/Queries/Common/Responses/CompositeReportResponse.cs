using PayVerse.Domain.Enums.Reports;
using PayVerse.Domain.ValueObjects.Reports;

namespace PayVerse.Application.Reports.Queries.Common.Responses;

public sealed record CompositeReportResponse(
    Guid Id,
    string Title,
    ReportPeriodResponse Period,
    ReportType Type,
    FileType FileType,
    ReportStatus Status,
    decimal TotalAmount,
    List<string> Summary,
    string FilePath,
    DateTime GeneratedAt);
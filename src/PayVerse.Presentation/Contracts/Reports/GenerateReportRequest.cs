using PayVerse.Domain.Enums.Reports;

namespace PayVerse.Presentation.Contracts.Reports;

public sealed record GenerateReportRequest(
    DateOnly StartDate,
    DateOnly EndDate,
    ReportType Type,
    FileType FileType);
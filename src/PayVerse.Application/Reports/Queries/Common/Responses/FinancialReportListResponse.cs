namespace PayVerse.Application.Reports.Queries.Common.Responses;

public sealed record FinancialReportListResponse(IReadOnlyList<FinancialReportResponse> FinancialReports);
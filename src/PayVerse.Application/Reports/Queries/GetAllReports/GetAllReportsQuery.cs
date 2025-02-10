using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetAllReports;

public sealed record GetAllReportsQuery() :IQuery<FinancialReportListResponse>;
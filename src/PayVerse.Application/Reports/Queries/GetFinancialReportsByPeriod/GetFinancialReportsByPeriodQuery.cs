using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetFinancialReportsByPeriod;

public sealed record GetFinancialReportsByPeriodQuery(
    DateOnly StartDate,
    DateOnly EndDate) : IQuery<FinancialReportListResponse>;
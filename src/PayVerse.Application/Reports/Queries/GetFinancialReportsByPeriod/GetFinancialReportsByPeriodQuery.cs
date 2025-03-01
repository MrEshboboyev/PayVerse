using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetCompositeFinancialReportsByPeriod;

public sealed record GetCompositeFinancialReportsByPeriodQuery(
    DateOnly StartDate,
    DateOnly EndDate) : IQuery<CompositeFinancialReportListResponse>;
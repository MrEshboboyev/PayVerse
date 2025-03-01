using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetCompositeFinancialReportById;

public sealed record GetCompositeFinancialReportByIdQuery(
    Guid ReportId) : IQuery<CompositeFinancialReportResponse>;
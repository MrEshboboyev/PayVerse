using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetFinancialReportById;

public sealed record GetFinancialReportByIdQuery(
    Guid ReportId) : IQuery<FinancialReportResponse>;
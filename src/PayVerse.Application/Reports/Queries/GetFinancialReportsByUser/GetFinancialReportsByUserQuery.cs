using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetCompositeFinancialReportsByUser;

public sealed record GetCompositeFinancialReportsByUserQuery(
    Guid UserId) : IQuery<CompositeFinancialReportListResponse>;
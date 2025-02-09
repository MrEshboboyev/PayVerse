using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetFinancialReportsByUser;

public sealed record GetFinancialReportsByUserQuery(
    Guid UserId) : IQuery<FinancialReportListResponse>;
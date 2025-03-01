using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Reports.Queries.Common.Responses;

namespace PayVerse.Application.Reports.Queries.GetCompositeReportById;

public sealed record GetCompositeReportByIdQuery(Guid Id) : IQuery<CompositeReportResponse>;

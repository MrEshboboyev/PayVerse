using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Invoices.Queries.GetTotalRevenueByUser;

public sealed record GetTotalRevenueByUserQuery(
    Guid UserId) : IQuery<decimal>;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Responses;

namespace PayVerse.Application.Payments.Queries.GetPaymentsByDateRange;

public sealed record GetPaymentsByDateRangeQuery(
    DateTime StartDate,
    DateTime EndDate) : IQuery<PaymentListResponse>;

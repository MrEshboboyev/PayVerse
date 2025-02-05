using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Responses;

namespace PayVerse.Application.Payments.Queries.GetAllPayments;

public sealed record GetAllPaymentsQuery() : IQuery<PaymentListResponse>;

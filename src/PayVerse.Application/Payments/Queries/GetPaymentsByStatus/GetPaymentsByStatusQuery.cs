using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Application.Payments.Queries.GetPaymentsByStatus;

public sealed record GetPaymentsByStatusQuery(
    PaymentStatus Status) : IQuery<PaymentListResponse>;

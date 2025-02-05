using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Responses;

namespace PayVerse.Application.Payments.Queries.GetPaymentsByUserId;

public sealed record GetPaymentsByUserIdQuery(
    Guid UserId) : IQuery<PaymentListResponse>;
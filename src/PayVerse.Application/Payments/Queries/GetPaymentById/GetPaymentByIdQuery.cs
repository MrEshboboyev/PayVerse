using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Responses;

namespace PayVerse.Application.Payments.Queries.GetPaymentById;

public sealed record GetPaymentByIdQuery(
    Guid PaymentId) : IQuery<PaymentResponse>;
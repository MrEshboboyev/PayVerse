using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Application.Payments.Queries.Common.Responses;

public sealed record PaymentResponse(
    Guid PaymentId,
    decimal PaymentAmount,
    PaymentStatus PaymentStatus,
    Guid UserId,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Presentation.Contracts.Payments;

public sealed record CreatePaymentRequest(
    decimal Amount,
    PaymentStatus Status);
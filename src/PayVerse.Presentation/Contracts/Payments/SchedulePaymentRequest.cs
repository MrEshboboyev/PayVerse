using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Presentation.Contracts.Payments;

public sealed record SchedulePaymentRequest(
    decimal Amount,
    PaymentStatus Status,
    DateTime ScheduledDate);
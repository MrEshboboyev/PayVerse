using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Application.Mementos.Payments;

public sealed record PaymentHistoryDto(
    int Version,
    DateTime CreatedAt,
    string Metadata,
    PaymentStatus Status,
    decimal Amount,
    string Currency);

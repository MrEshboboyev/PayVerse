using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentStatusUpdatedDomainEvent(
    Guid Id,
    Guid PaymentId,
    PaymentStatus OldStatus,
    PaymentStatus NewStatus) : DomainEvent(Id);
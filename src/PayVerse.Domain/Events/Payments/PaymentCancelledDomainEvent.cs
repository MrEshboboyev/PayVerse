namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentCancelledDomainEvent(
    Guid Id,
    Guid PaymentId) : DomainEvent(Id);

namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentProcessedDomainEvent(
    Guid Id,
    Guid PaymentId) : DomainEvent(Id);

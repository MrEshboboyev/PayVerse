namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentProcessingDomainEvent(
    Guid Id,
    Guid PaymentId) : DomainEvent(Id);

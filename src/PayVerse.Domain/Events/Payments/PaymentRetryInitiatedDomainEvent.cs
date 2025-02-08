namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentRetryInitiatedDomainEvent(
    Guid Id,
    Guid PaymentId) : DomainEvent(Id);
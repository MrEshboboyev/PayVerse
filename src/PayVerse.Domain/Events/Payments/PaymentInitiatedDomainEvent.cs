namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentInitiatedDomainEvent(
    Guid Id,
    Guid PaymentId) : DomainEvent(Id);
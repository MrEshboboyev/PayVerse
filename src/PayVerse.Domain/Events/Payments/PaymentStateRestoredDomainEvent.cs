namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentStateRestoredDomainEvent(
    Guid Id,
    Guid PaymentId) : DomainEvent(Id);

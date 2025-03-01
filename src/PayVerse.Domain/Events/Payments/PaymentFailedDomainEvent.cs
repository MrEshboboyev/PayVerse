namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentFailedDomainEvent(
    Guid Id,
    Guid PaymentId, 
    string Reason) : DomainEvent(Id);

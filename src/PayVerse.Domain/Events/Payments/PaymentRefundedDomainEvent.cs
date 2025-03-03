namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentRefundedDomainEvent(
    Guid Id,
    Guid PaymentId,
    string Reason) : DomainEvent(Id);
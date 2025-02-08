namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentScheduledDomainEvent(
    Guid Id,
    Guid PaymentId,
    DateTime ScheduledDate) : DomainEvent(Id);
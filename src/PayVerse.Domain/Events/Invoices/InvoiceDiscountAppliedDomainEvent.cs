namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceDiscountAppliedDomainEvent(
    Guid Id,
    Guid InvoiceId,
    decimal DiscountAmount) : DomainEvent(Id);
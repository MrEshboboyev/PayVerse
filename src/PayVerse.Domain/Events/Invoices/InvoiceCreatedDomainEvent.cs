namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceCreatedDomainEvent(
    Guid Id,
    Guid InvoiceId) : DomainEvent(Id);
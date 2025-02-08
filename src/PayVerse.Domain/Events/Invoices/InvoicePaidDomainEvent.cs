namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoicePaidDomainEvent(
    Guid Id,
    Guid InvoiceId) : DomainEvent(Id);
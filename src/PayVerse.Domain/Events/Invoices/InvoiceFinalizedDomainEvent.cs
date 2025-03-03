namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceFinalizedDomainEvent(
    Guid Id,
    Guid InvoiceId) : DomainEvent(Id);
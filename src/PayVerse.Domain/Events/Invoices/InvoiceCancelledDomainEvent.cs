namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceCancelledDomainEvent(
    Guid Id,
    Guid InvoiceId,
    string Reason) : DomainEvent(Id);
namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceItemRemovedDomainEvent(
    Guid Id,
    Guid InvoiceId,
    decimal Amount) : DomainEvent(Id);
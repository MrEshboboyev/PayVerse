namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceOverdueDomainEvent(
    Guid Id,
    Guid InvoiceId) : DomainEvent(Id);
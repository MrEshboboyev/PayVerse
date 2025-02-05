namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceItemAddedDomainEvent(
    Guid Id,
    Guid InvoiceId,
    Guid ItemId) : DomainEvent(Id);
namespace PayVerse.Domain.Events.Invoices;

public sealed record InvoiceTaxAddedDomainEvent(
    Guid Id,
    Guid InvoiceId,
    decimal TaxAmount) : DomainEvent(Id);
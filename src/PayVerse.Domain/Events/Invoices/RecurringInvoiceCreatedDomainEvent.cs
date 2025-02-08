namespace PayVerse.Domain.Events.Invoices;

public sealed record RecurringInvoiceCreatedDomainEvent(
    Guid Id,
    Guid InvoiceId,
    int FrequencyInMonths) : DomainEvent(Id);
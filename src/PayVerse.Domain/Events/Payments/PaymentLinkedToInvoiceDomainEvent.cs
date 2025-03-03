namespace PayVerse.Domain.Events.Payments;

public sealed record PaymentLinkedToInvoiceDomainEvent(
    Guid Id,
    Guid PaymentId,
    Guid InvoiceId) : DomainEvent(Id);

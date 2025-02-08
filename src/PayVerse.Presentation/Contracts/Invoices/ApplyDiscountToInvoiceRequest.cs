namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record ApplyDiscountToInvoiceRequest(
    Guid InvoiceId,
    decimal DiscountAmount);
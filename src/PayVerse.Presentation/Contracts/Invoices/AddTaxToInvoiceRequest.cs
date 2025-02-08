namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record AddTaxToInvoiceRequest(
    Guid InvoiceId,
    decimal TaxAmount);
namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record CreateInvoiceRequest(
    string InvoiceNumber,
    DateTime Date,
    decimal TotalAmount);
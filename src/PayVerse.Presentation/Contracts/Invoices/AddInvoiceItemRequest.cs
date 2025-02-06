namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record AddInvoiceItemRequest(
    string Description,
    decimal Amount);
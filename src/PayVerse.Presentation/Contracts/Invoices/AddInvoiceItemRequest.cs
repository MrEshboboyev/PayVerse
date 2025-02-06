namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record AddInvoiceItemRequest(
    string Description,
    int Quantity,
    decimal Amount);
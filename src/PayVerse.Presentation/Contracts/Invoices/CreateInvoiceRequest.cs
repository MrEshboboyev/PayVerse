namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record CreateInvoiceRequest(
    List<AddInvoiceItemRequest> Items);
namespace PayVerse.Presentation.Contracts.Invoices;

public sealed record SendInvoiceToClientRequest(
    Guid InvoiceId,
    string Email);
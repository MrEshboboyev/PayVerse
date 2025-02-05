namespace PayVerse.Application.Invoices.Queries.Common.Responses;

public sealed record InvoiceItemResponse(
    Guid InvoiceItemId,
    string Description,
    decimal Amount);
namespace PayVerse.Application.Invoices.Queries.Common.Responses;

public sealed record InvoiceWithItemsResponse(
    InvoiceResponse Invoice,
    IReadOnlyList<InvoiceItemResponse> Items);
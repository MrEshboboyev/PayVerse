namespace PayVerse.Application.Invoices.Queries.Common.Responses;

public sealed record InvoiceItemListResponse(
    IReadOnlyList<InvoiceItemResponse> InvoiceItems);
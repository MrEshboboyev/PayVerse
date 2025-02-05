namespace PayVerse.Application.Invoices.Queries.Common.Responses;

public sealed record InvoiceListResponse(IReadOnlyList<InvoiceResponse> Invoices);
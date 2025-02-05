namespace PayVerse.Application.Invoices.Queries.Common.Responses;

public sealed record InvoiceResponse(
    Guid InvoiceId,
    string InvoiceNumber,
    DateTime InvoiceDate,
    decimal TotalAmount,
    Guid UserId);
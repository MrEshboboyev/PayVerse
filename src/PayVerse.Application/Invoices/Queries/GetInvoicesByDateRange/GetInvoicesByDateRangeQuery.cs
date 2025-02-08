using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetInvoicesByDateRange;

public sealed record GetInvoicesByDateRangeQuery(
    DateTime StartDate,
    DateTime EndDate) : IQuery<InvoiceListResponse>;
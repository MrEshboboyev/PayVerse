using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceItems;

public sealed record GetInvoiceItemsQuery(
    Guid InvoiceId) : IQuery<InvoiceItemListResponse>;
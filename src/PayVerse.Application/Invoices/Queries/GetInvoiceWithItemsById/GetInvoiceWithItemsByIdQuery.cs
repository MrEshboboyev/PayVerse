using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceWithItemsById;

public sealed record GetInvoiceWithItemsByIdQuery(
    Guid InvoiceId) : IQuery<InvoiceWithItemsResponse>;

using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceItemById;

public sealed record GetInvoiceItemByIdQuery(
    Guid InvoiceId,
    Guid ItemId) : IQuery<InvoiceItemResponse>;
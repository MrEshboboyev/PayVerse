using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceById;

public sealed record GetInvoiceByIdQuery(
    Guid InvoiceId) : IQuery<InvoiceResponse>;
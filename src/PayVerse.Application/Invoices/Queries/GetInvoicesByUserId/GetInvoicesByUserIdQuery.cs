using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetInvoicesByUserId;

public sealed record GetInvoicesByUserIdQuery(
    Guid UserId) : IQuery<InvoiceListResponse>;
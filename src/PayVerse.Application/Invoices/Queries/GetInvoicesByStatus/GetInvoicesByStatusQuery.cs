using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Enums.Invoices;

namespace PayVerse.Application.Invoices.Queries.GetInvoicesByStatus;

public sealed record GetInvoicesByStatusQuery(
    InvoiceStatus Status) : IQuery<InvoiceListResponse>;
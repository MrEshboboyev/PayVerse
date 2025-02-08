using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetOverdueInvoices;

public sealed record GetOverdueInvoicesQuery() : IQuery<InvoiceListResponse>;
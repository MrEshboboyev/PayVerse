using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Responses;

namespace PayVerse.Application.Invoices.Queries.GetAllInvoices;

public sealed record GetAllInvoicesQuery() : IQuery<InvoiceListResponse>;
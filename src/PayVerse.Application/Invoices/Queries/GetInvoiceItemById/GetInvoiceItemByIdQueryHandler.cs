using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceItemById;

internal sealed class GetInvoiceItemByIdQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoiceItemByIdQuery, InvoiceItemResponse>
{
    public async Task<Result<InvoiceItemResponse>> Handle(
        GetInvoiceItemByIdQuery request,
        CancellationToken cancellationToken)
    {
        var (invoiceId, itemId) = request;
        
        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure<InvoiceItemResponse>(
                DomainErrors.Invoice.NotFound(invoiceId));
        }

        var invoiceItem = invoice.GetItemById(itemId);
        if (invoiceItem is null)
        {
            return Result.Failure<InvoiceItemResponse>(
                DomainErrors.Invoice.ItemNotFound(itemId));
        }

        var response = InvoiceItemResponseFactory.Create(invoiceItem);

        return Result.Success(response);
    }
}

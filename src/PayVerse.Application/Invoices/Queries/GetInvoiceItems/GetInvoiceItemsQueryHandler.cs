using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceItems;

internal sealed class GetInvoiceItemsQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoiceItemsQuery, InvoiceItemListResponse>
{
    public async Task<Result<InvoiceItemListResponse>> Handle(
        GetInvoiceItemsQuery request,
        CancellationToken cancellationToken)
    {
        var invoiceId = request.InvoiceId;
        
        #region Get Invoice

        var invoice = await invoiceRepository.GetByIdWithItemsAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure<InvoiceItemListResponse>(
                DomainErrors.Invoice.NotFound(invoiceId));
        }

        #endregion
        
        #region Prepare response

        var response = new InvoiceItemListResponse(
            invoice.Items
            .Select(InvoiceItemResponseFactory.Create)
            .ToList());
        
        #endregion

        return Result.Success(response);
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceWithItemsById;

internal sealed class GetInvoiceWithItemsByIdQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoiceWithItemsByIdQuery, InvoiceWithItemsResponse>
{
    public async Task<Result<InvoiceWithItemsResponse>> Handle(
        GetInvoiceWithItemsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var invoiceId = request.InvoiceId;
        
        // Get Invoice
        var invoiceWithItems = await invoiceRepository.GetByIdWithItemsAsync(
            invoiceId,
            cancellationToken);
        if (invoiceWithItems is null)
        {
            return Result.Failure<InvoiceWithItemsResponse>(
                DomainErrors.Invoice.NotFound(invoiceId));
        }
        
        // Prepare response
        var response = new InvoiceWithItemsResponse
        (
            InvoiceResponseFactory.Create(invoiceWithItems),
            invoiceWithItems.Items.Select(InvoiceItemResponseFactory.Create)
                .ToList().AsReadOnly()
        );

        return Result.Success(response);
    }
}

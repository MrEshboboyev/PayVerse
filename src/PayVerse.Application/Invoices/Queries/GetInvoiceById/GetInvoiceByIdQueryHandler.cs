using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetInvoiceById;

internal sealed class GetInvoiceByIdQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse>
{
    public async Task<Result<InvoiceResponse>> Handle(
        GetInvoiceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var invoiceId = request.InvoiceId;
        
        #region Get Invoice

        var invoice = await invoiceRepository.GetByIdAsync(invoiceId, cancellationToken);
        if (invoice is null)
        {
            return Result.Failure<InvoiceResponse>(
                DomainErrors.Invoice.NotFound(invoiceId));
        }

        #endregion
        
        #region Prepare response
        
        var response = InvoiceResponseFactory.Create(invoice);
        
        #endregion

        return Result.Success(response);
    }
}
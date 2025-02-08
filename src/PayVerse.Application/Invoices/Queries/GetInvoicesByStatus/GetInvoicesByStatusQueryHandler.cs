using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetInvoicesByStatus;

internal sealed class GetInvoicesByStatusQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoicesByStatusQuery, InvoiceListResponse>
{
    public async Task<Result<InvoiceListResponse>> Handle(
        GetInvoicesByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var status = request.Status;
        
        var invoices = await invoiceRepository.GetByStatusAsync(
            status,
            cancellationToken);
        
        var response = new InvoiceListResponse(
            invoices
                .Select(InvoiceResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetOverdueInvoices;

internal sealed class GetOverdueInvoicesQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetOverdueInvoicesQuery,InvoiceListResponse>
{
    public async Task<Result<InvoiceListResponse>> Handle(
        GetOverdueInvoicesQuery request,
        CancellationToken cancellationToken)
    {
        var invoices = await invoiceRepository.GetOverdueAsync(cancellationToken);
        
        var response = new InvoiceListResponse(
            invoices
                .Select(InvoiceResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}

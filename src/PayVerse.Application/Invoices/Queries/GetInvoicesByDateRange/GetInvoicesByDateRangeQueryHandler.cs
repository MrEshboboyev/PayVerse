using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetInvoicesByDateRange;

internal sealed class GetInvoicesByDateRangeQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoicesByDateRangeQuery, InvoiceListResponse>
{
    public async Task<Result<InvoiceListResponse>> Handle(
        GetInvoicesByDateRangeQuery request,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request;
        
        var invoices = await invoiceRepository.GetByDateRangeAsync(
            startDate,
            endDate,
            cancellationToken);
        
        var response = new InvoiceListResponse(
            invoices
                .Select(InvoiceResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}
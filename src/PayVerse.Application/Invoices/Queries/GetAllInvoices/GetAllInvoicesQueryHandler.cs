using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetAllInvoices;

internal sealed class GetAllInvoicesQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetAllInvoicesQuery, InvoiceListResponse>
{
    public async Task<Result<InvoiceListResponse>> Handle(
        GetAllInvoicesQuery request,
        CancellationToken cancellationToken)
    {
        var invoices = await invoiceRepository.GetAllAsync(cancellationToken);

        var response = new InvoiceListResponse(invoices
            .Select(InvoiceResponseFactory.Create)
            .ToList()
            .AsReadOnly());

        return Result.Success(response);
    }
}

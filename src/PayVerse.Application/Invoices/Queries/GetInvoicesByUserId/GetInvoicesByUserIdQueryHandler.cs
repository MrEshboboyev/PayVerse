using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Invoices.Queries.Common.Factories;
using PayVerse.Application.Invoices.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetInvoicesByUserId;

internal sealed class GetInvoicesByUserIdQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoicesByUserIdQuery, InvoiceListResponse>
{
    public async Task<Result<InvoiceListResponse>> Handle(
        GetInvoicesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var invoices = await invoiceRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);
        
        var response = new InvoiceListResponse(
            invoices
                .Select(InvoiceResponseFactory.Create)
                .ToList()
                .AsReadOnly());

        return Result.Success(response);
    }
}

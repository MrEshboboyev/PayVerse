using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Repositories.Invoices;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Invoices.Queries.GetTotalRevenueByUser;

internal sealed class GetTotalRevenueByUserQueryHandler(
    IInvoiceRepository invoiceRepository) : IQueryHandler<GetTotalRevenueByUserQuery, decimal>
{
    public async Task<Result<decimal>> Handle(
        GetTotalRevenueByUserQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        var totalRevenue = await invoiceRepository.GetTotalRevenueByUserAsync(
            userId,
            cancellationToken);
        
        return Result.Success(totalRevenue);
    }
}
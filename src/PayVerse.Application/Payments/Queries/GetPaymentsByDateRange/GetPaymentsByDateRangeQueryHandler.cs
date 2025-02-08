using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Factories;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Queries.GetPaymentsByDateRange;

internal sealed class GetPaymentsByDateRangeQueryHandler(
    IPaymentRepository paymentRepository) : IQueryHandler<GetPaymentsByDateRangeQuery, PaymentListResponse>
{
    public async Task<Result<PaymentListResponse>> Handle(
        GetPaymentsByDateRangeQuery request,
        CancellationToken cancellationToken)
    {
        var (startDate, endDate) = request;
        
        var payments = await paymentRepository.GetByDateRangeAsync(
            startDate, 
            endDate,
            cancellationToken);
        
        var response = new PaymentListResponse(
            payments
                .Select(PaymentResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}
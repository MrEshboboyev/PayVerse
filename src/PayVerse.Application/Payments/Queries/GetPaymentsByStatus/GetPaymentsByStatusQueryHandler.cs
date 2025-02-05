using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Factories;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Queries.GetPaymentsByStatus;

internal sealed class GetPaymentsByStatusQueryHandler(
    IPaymentRepository paymentRepository) : IQueryHandler<GetPaymentsByStatusQuery, PaymentListResponse>
{
    public async Task<Result<PaymentListResponse>> Handle(
        GetPaymentsByStatusQuery request,
        CancellationToken cancellationToken)
    {
        var status = request.Status;
        
        var payments = await paymentRepository.GetByStatusAsync(
            status,
            cancellationToken);
        
        var response = new PaymentListResponse(
            payments
                .Select(PaymentResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}

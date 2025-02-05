using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Factories;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Queries.GetAllPayments;

internal sealed class GetAllPaymentsQueryHandler(
    IPaymentRepository paymentRepository) : IQueryHandler<GetAllPaymentsQuery,PaymentListResponse>
{
    public async Task<Result<PaymentListResponse>> Handle(
        GetAllPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var payments = await paymentRepository.GetAllAsync(cancellationToken);
        
        var response = new PaymentListResponse(
            payments
                .Select(PaymentResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}

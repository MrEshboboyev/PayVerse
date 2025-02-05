using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Factories;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Queries.GetPaymentsByUserId;

internal sealed class GetPaymentsByUserIdQueryHandler(
    IPaymentRepository paymentRepository) : IQueryHandler<GetPaymentsByUserIdQuery, PaymentListResponse>
{
    public async Task<Result<PaymentListResponse>> Handle(
        GetPaymentsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        #region Get Payments by User ID

        var payments = await paymentRepository.GetAllByUserIdAsync(
            userId, 
            cancellationToken);

        #endregion
        
        #region Prepare response

        var response = new PaymentListResponse(
            payments
                .Select(PaymentResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        #endregion

        return Result.Success(response);
    }
}
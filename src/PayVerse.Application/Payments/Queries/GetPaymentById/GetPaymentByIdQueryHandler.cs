using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Queries.Common.Factories;
using PayVerse.Application.Payments.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Queries.GetPaymentById;

internal sealed class GetPaymentByIdQueryHandler(
    IPaymentRepository paymentRepository) : IQueryHandler<GetPaymentByIdQuery, PaymentResponse>
{
    public async Task<Result<PaymentResponse>> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var paymentId = request.PaymentId;
        
        #region Get Payment

        var payment = await paymentRepository.GetByIdAsync(
            paymentId, 
            cancellationToken);
        if (payment is null)
        {
            return Result.Failure<PaymentResponse>(
                DomainErrors.Payment.NotFound(paymentId));
        }

        #endregion
        
        #region Prepare response
        
        var response = PaymentResponseFactory.Create(payment);
        
        #endregion

        return Result.Success(response);
    }
}
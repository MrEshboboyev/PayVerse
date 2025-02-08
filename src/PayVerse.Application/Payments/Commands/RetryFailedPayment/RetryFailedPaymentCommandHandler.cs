using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Commands.RetryFailedPayment;

internal sealed class RetryFailedPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RetryFailedPaymentCommand>
{
    public async Task<Result> Handle(
        RetryFailedPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var paymentId = request.PaymentId;
        
        #region Get this Payment
        
        var payment = await paymentRepository.GetByIdAsync(
            paymentId,
            cancellationToken);
        if (payment is null)
        {
            return Result.Failure(
                DomainErrors.Payment.NotFound(paymentId));
        }
        
        #endregion
        
        #region Update this Payment

        var updateStatusResult = payment.UpdateStatus(PaymentStatus.Pending);
        if (updateStatusResult.IsFailure)
        {
            return Result.Failure(updateStatusResult.Error);
        }
        
        #endregion

        await paymentRepository.UpdateAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
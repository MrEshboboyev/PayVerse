using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Commands.RefundPayment;

internal sealed class RefundPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RefundPaymentCommand>
{
    public async Task<Result> Handle(
        RefundPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var (paymentId, refundTransactionId, reason) = request;
        
        #region Get this payment
        
        var payment = await paymentRepository.GetByIdAsync(
            paymentId,
            cancellationToken);
        if (payment is null)
        {
            return Result.Failure(
                DomainErrors.Payment.NotFound(paymentId));
        }
        
        #endregion
        
        #region Refund payment

        var refundResult = payment.Refund(
            refundTransactionId,
            reason);
        if (refundResult.IsFailure)
        {
            return Result.Failure(refundResult.Error);
        }
        
        #endregion

        await paymentRepository.UpdateAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
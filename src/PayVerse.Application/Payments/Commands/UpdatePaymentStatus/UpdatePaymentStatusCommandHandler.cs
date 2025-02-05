using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Commands.UpdatePaymentStatus;

internal sealed class UpdatePaymentStatusCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdatePaymentStatusCommand>
{
    public async Task<Result> Handle(
        UpdatePaymentStatusCommand request,
        CancellationToken cancellationToken)
    {
        var (paymentId, paymentStatus) = request;
        
        #region Get Payment

        var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment is null)
        {
            return Result.Failure(
                DomainErrors.Payment.NotFound(paymentId));
        }

        #endregion

        #region Update Payment Status

        var updateStatusResult = payment.UpdateStatus(paymentStatus);
        if (updateStatusResult.IsFailure)
        {
            return Result.Failure(
                updateStatusResult.Error);
        }

        #endregion

        #region Save Changes

        await paymentRepository.UpdateAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
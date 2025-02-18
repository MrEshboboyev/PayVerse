using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Payments.Commands.SchedulePayment;

internal sealed class SchedulePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SchedulePaymentCommand>
{
    public async Task<Result> Handle(
        SchedulePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var (amount, paymentStatus, userId, scheduledDate) = request;

        #region Prepare value objects
        
        var amountResult = Amount.Create(amount);
        if (amountResult.IsFailure)
        {
            return Result.Failure(amountResult.Error);
        }
        
        #endregion
        
        #region Create new payment

        var payment = Payment.Create(
            Guid.NewGuid(),
            amountResult.Value,
            paymentStatus,
            userId,
            scheduledDate);
        
        #endregion

        await paymentRepository.AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
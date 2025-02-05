using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Payments;

namespace PayVerse.Application.Payments.Commands.CreatePayment;

internal sealed class CreatePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreatePaymentCommand>
{
    public async Task<Result> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var (amount, paymentStatus, userId) = request;
        
        #region Prepare value objects

        var amountResult = PaymentAmount.Create(amount);
        if (amountResult.IsFailure)
        {
            return Result.Failure(
                amountResult.Error);
        }
        
        #endregion
        
        #region Create Payment

        var payment = Payment.Create(
            Guid.NewGuid(),
            amountResult.Value,
            paymentStatus,
            userId);

        #endregion

        #region Add Payment to Repository

        await paymentRepository.AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
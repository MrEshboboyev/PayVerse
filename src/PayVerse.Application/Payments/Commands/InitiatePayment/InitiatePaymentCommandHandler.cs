using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Application.Payments.Commands.InitiatePayment;

internal sealed class InitiatePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<InitiatePaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        InitiatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var (amountValue, userId, invoiceId, paymentMethod, scheduledDate) = request;

        var amountResult = Amount.Create(amountValue);
        if (amountResult.IsFailure)
        {
            return Result.Failure<Guid>(amountResult.Error);
        }

        var payment = Payment.Create(
            Guid.NewGuid(),
            amountResult.Value,
            PaymentStatus.Pending,
            userId,
            scheduledDate);

        if (invoiceId.HasValue)
        {
            payment.LinkToInvoice(invoiceId.Value);
        }

        var setPaymentMethodResult = payment.SetPaymentMethod(paymentMethod);
        if (setPaymentMethodResult.IsFailure)
        {
            return Result.Failure<Guid>(setPaymentMethodResult.Error);
        }

        await paymentRepository.AddAsync(payment, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(payment.Id);
    }
}

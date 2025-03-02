using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Commands.ProcessPaymentWithDecorators;

internal sealed class ProcessPaymentWithDecoratorsCommandHandler(
    IPaymentProcessor paymentProcessor,
    IPaymentRepository paymentRepository) : ICommandHandler<ProcessPaymentWithDecoratorsCommand, PaymentResult>
{
    public async Task<Result<PaymentResult>> Handle(
        ProcessPaymentWithDecoratorsCommand request,
        CancellationToken cancellationToken)
    {
        var (paymentId, amount, currency, provider, paymentDetails, applyFraudCheck, applyLimitCheck) = request;

        var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);

        if (payment is null)
        {
            return Result.Failure<PaymentResult>(
                DomainErrors.Payment.NotFound(paymentId));
        }

        // The payment processor is already decorated with all necessary functionality
        return await paymentProcessor.ProcessPaymentAsync(payment, cancellationToken);
    }
}

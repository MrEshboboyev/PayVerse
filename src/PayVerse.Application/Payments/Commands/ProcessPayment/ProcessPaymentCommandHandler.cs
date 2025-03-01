using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Bridges;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Commands.ProcessPayment;

internal sealed class ProcessPaymentCommandHandler(
    IPaymentRepository paymentRepository,
    Func<PaymentProcessorType, IPaymentProvider, PaymentProcessor> paymentProcessorFactory,
    IEnumerable<IPaymentProvider> paymentProviders) : ICommandHandler<ProcessPaymentCommand, PaymentDto>
{
    public async Task<Result<PaymentDto>> Handle(
        ProcessPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var (paymentId, providerName, paymentDetails) = request;

        #region Get this Payment

        var payment = await paymentRepository.GetByIdAsync(paymentId, cancellationToken);
        if (payment is null)
        {
            return Result.Failure<PaymentDto>(
                DomainErrors.Payment.NotFound(paymentId));
        }

        #endregion

        #region Get Payment provider

        // Select the appropriate payment provider
        var paymentProvider = paymentProviders.FirstOrDefault(p => p.GetProviderName() == providerName);
        if (paymentProvider is null)
        {
            return Result.Failure<PaymentDto>(
                DomainErrors.PaymentProvider.NotFound(providerName));
        }

        #endregion

        #region Process Payment

        // Determine if this is a recurring payment
        var processorType = payment.ScheduledDate.HasValue
            ? PaymentProcessorType.Recurring
            : PaymentProcessorType.Standard;

        // Create the appropriate payment processor using the factory
        var paymentProcessor = paymentProcessorFactory(processorType, paymentProvider);

        // Process the payment
        var processedPayment = await paymentProcessor.ProcessPaymentAsync(payment, paymentDetails);

        #endregion

        return Result.Success(new PaymentDto(
            payment.Id,
            providerName,
            DateTime.UtcNow,
            payment.Amount.Value,
            true,
            string.Empty
        ));
    }
}

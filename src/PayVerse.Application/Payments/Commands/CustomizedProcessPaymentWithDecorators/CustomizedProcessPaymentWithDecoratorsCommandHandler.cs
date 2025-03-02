using MediatR;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Payments.Factories;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Payments.Commands.CustomizedProcessPaymentWithDecorators;

internal sealed class CustomizedProcessPaymentCommandHandler : ICommandHandler<CustomizedProcessPaymentCommand, PaymentResult>
{
    private readonly IPaymentProcessorFactory _paymentProcessorFactory;
    private readonly IPaymentRepository _paymentRepository;

    public CustomizedProcessPaymentCommandHandler(
        IPaymentProcessorFactory paymentProcessorFactory,
        IPaymentRepository paymentRepository)
    {
        _paymentProcessorFactory = paymentProcessorFactory;
        _paymentRepository = paymentRepository;
    }

    public async Task<Result<PaymentResult>> Handle(CustomizedProcessPaymentCommand request,
                                                    CancellationToken cancellationToken)
    {
        var (paymentId, amount, enableNotifications) = request;

        var payment = await _paymentRepository.GetByIdAsync(paymentId, cancellationToken);

        if (payment is null)
        {
            return Result.Failure<PaymentResult>(
                DomainErrors.Payment.NotFound(paymentId));
        }

        // Create a processor with custom options
        var options = new PaymentProcessorOptions
        {
            EnableFraudDetection = amount > 1000, // Only use fraud detection for large payments
            EnableNotifications = enableNotifications
        };

        var processor = _paymentProcessorFactory.CreatePaymentProcessor(options);

        // Process the payment with the configured processor
        return await processor.ProcessPaymentAsync(payment, cancellationToken);
    }
}

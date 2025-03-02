using PayVerse.Application.Common.Interfaces.Payments;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Application.Payments.Decorators;

/// <summary>
/// Adds validation logic to payment processing
/// </summary>
public class ValidationPaymentDecorator(
    IPaymentProcessor paymentProcessor,
    IPaymentValidationService validationService) : PaymentProcessorDecorator(paymentProcessor)
{
    public override async Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        // Perform validation before processing
        // only validate payment amount - fix this coming soon
        var isValidAmount = await validationService.ValidatePaymentAmountAsync(
            payment.Amount.Value, 
            "USD", 
            cancellationToken);

        if (!isValidAmount)
        {
            return new PaymentResult(false, null, "Payment amount is not valid");
        }

        // If validation passes, continue to the decorated component
        return await base.ProcessPaymentAsync(payment, cancellationToken);
    }
}

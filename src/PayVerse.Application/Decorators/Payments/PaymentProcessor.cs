using PayVerse.Domain.Adapters.Payments;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Repositories;

namespace PayVerse.Application.Decorators.Payments;

/// <summary>
/// Basic implementation of the payment processor
/// </summary>
public class PaymentProcessor(
    IPaymentGatewayAdapter paymentGateway,
    IUnitOfWork unitOfWork) : IPaymentProcessor
{
    public async Task<PaymentResult> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        // Basic payment processing logic
        var result = await paymentGateway.ProcessPaymentAsync(payment);

        // Update payment with transaction details
        payment.MarkAsProcessed(result.TransactionId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new PaymentResult(true, result.TransactionId, "Payment processed successfully");
    }
}

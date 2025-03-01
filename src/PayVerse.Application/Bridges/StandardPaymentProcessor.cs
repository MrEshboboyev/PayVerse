using Microsoft.Extensions.Logging;
using PayVerse.Domain.Bridges;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories.Payments;

namespace PayVerse.Application.Bridges;

/// <summary>
/// The Refined Abstraction extends the interface defined by Abstraction.
/// </summary>
public class StandardPaymentProcessor(
    IPaymentProvider paymentProvider,
    IPaymentRepository paymentRepository,
    ILogger<StandardPaymentProcessor> logger) : PaymentProcessor(paymentProvider)
{
    public override async Task<Payment> ProcessPaymentAsync(Payment payment,
                                                          IDictionary<string, string> paymentDetails)
    {
        logger.LogInformation("Processing payment {PaymentId} using {Provider}", payment.Id, _paymentProvider.GetProviderName());
        try
        {
            // Process the payment using the provider
            string transactionId = await _paymentProvider.ProcessPaymentAsync(
                payment.Id,
                payment.Amount.Value,
                "USD", // Hardcoded currency for now
                paymentDetails);
            // Update the payment with the transaction information
            payment.SetTransactionId(transactionId);
            payment.SetProviderName(_paymentProvider.GetProviderName());
            payment.MarkAsProcessing();
            // Save the updated payment
            await paymentRepository.UpdateAsync(payment);
            return payment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process payment {PaymentId}", payment.Id);
            // Update the payment to reflect the failure
            payment.MarkAsFailed(ex.Message);
            await paymentRepository.UpdateAsync(payment);
            throw;
        }
    }

    public override async Task<Payment> RefundPaymentAsync(Payment payment, decimal amount)
    {
        if (payment.Status != PaymentStatus.Processed)
        {
            throw new InvalidOperationException($"Cannot refund payment {payment.Id} with status {payment.Status}");
        }
        try
        {
            bool success = await _paymentProvider.RefundPaymentAsync(
                payment.TransactionId,
                amount,
                "USD"); // Refund in the same currency as the payment
            if (success)
            {
                // Record the refund details on the payment
                payment.MarkAsRefunded();
                await paymentRepository.UpdateAsync(payment);
            }
            else
            {
                throw new InvalidOperationException($"Refund failed for payment {payment.Id}");
            }
            return payment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to refund payment {PaymentId}", payment.Id);
            throw;
        }
    }

    public override async Task<Payment> CheckPaymentStatusAsync(Payment payment)
    {
        if (string.IsNullOrEmpty(payment.TransactionId))
        {
            throw new InvalidOperationException($"Payment {payment.Id} has no transaction ID");
        }
        try
        {
            PaymentStatus providerStatus = await _paymentProvider.CheckPaymentStatusAsync(payment.TransactionId);
            // Update the payment status if it has changed
            if (payment.Status != providerStatus && providerStatus != PaymentStatus.Unknown)
            {
                payment.UpdateStatus(providerStatus);
                await paymentRepository.UpdateAsync(payment);
            }
            return payment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to check status for payment {PaymentId}", payment.Id);
            throw;
        }
    }
}
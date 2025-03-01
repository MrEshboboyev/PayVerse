using Microsoft.Extensions.Logging;
using PayVerse.Domain.Bridges;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Repositories.Payments;

namespace PayVerse.Application.Bridges;

public class RecurringPaymentProcessor(
    IPaymentProvider paymentProvider,
    IPaymentRepository paymentRepository,
    ILogger<RecurringPaymentProcessor> logger,
    ILoggerFactory loggerFactory) : PaymentProcessor(paymentProvider)
{
    public override async Task<Payment> ProcessPaymentAsync(Payment payment, IDictionary<string, string> paymentDetails)
    {
        if (!_paymentProvider.SupportsRecurringPayments())
        {
            throw new NotSupportedException($"Payment provider {_paymentProvider.GetProviderName()} does not support recurring payments");
        }

        logger.LogInformation("Processing recurring payment {PaymentId} using {Provider}", payment.Id, _paymentProvider.GetProviderName());

        // Add subscription or recurring payment specific details
        paymentDetails["isRecurring"] = "true";

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

            // Schedule next payment if this is part of a recurring series
            // This would typically be handled by a separate scheduling service

            // Save the updated payment
            await paymentRepository.UpdateAsync(payment);

            return payment;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to process recurring payment {PaymentId}", payment.Id);

            // Update the payment to reflect the failure
            payment.MarkAsFailed(ex.Message);
            await paymentRepository.UpdateAsync(payment);

            throw;
        }
    }

    public override async Task<Payment> RefundPaymentAsync(Payment payment, decimal amount)
    {
        // Similar implementation to StandardPaymentProcessor but with recurring-specific logic
        // For example, you might need to cancel future payments if refunding a recurring payment

        if (payment.Status != PaymentStatus.Processed)
        {
            throw new InvalidOperationException($"Cannot refund payment {payment.Id} with status {payment.Status}");
        }

        try
        {
            bool success = await _paymentProvider.RefundPaymentAsync(
                payment.TransactionId,
                amount,
                "USD"); // Hardcoded currency for now

            if (success)
            {
                // Record the refund details on the payment
                payment.MarkAsRefunded();

                // Cancel any future scheduled payments in the series
                // This would be handled by a separate service

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
            logger.LogError(ex, "Failed to refund recurring payment {PaymentId}", payment.Id);
            throw;
        }
    }

    public override async Task<Payment> CheckPaymentStatusAsync(Payment payment)
    {
        // Create a new instance of StandardPaymentProcessor using the ILoggerFactory
        // to create a correctly typed logger for StandardPaymentProcessor
        var standardProcessorLogger = loggerFactory.CreateLogger<StandardPaymentProcessor>();

        var standardProcessor = new StandardPaymentProcessor(
            _paymentProvider,
            paymentRepository,
            standardProcessorLogger);

        return await standardProcessor.CheckPaymentStatusAsync(payment);
    }
}
using Microsoft.Extensions.Logging;
using PayVerse.Domain.Bridges;
using PayVerse.Domain.Enums.Payments;
using Stripe;

namespace PayVerse.Infrastructure.Payments.Providers;

public class StripePaymentProvider(IStripeClient stripeClient, ILogger<StripePaymentProvider> logger) : IPaymentProvider
{
    public async Task<string> ProcessPaymentAsync(Guid paymentId, decimal amount, string currency, IDictionary<string, string> paymentDetails)
    {
        try
        {
            logger.LogInformation("Processing payment {PaymentId} via Stripe", paymentId);

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe uses cents
                Currency = currency.ToLower(),
                PaymentMethod = paymentDetails["paymentMethodId"],
                Confirm = true,
                ReturnUrl = paymentDetails.TryGetValue("returnUrl", out string? value) ? value : null, // Fix here
                Description = $"Payment {paymentId}"
            };

            var service = new PaymentIntentService(stripeClient);
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing Stripe payment {PaymentId}", paymentId);
            throw new Exception("Failed to process payment through Stripe", ex);
        }
    }

    public async Task<bool> RefundPaymentAsync(string transactionId, decimal amount, string currency)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = transactionId,
                Amount = (long)(amount * 100) // Stripe uses cents
            };

            var service = new RefundService(stripeClient);
            var refund = await service.CreateAsync(options);

            return refund.Status == "succeeded";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error refunding Stripe payment {TransactionId}", transactionId);
            return false;
        }
    }

    public async Task<PaymentStatus> CheckPaymentStatusAsync(string transactionId)
    {
        try
        {
            var service = new PaymentIntentService(stripeClient);
            var paymentIntent = await service.GetAsync(transactionId);

            return paymentIntent.Status switch
            {
                "succeeded" => PaymentStatus.Processed,
                "processing" => PaymentStatus.Processing,
                "requires_payment_method" => PaymentStatus.Failed,
                "requires_action" => PaymentStatus.Pending,
                "canceled" => PaymentStatus.Cancelled,
                _ => PaymentStatus.Unknown
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking Stripe payment status {TransactionId}", transactionId);
            return PaymentStatus.Unknown;
        }
    }

    public bool SupportsRecurringPayments() => true;

    public bool SupportsPartialRefunds() => true;

    public string GetProviderName() => "Stripe";
}


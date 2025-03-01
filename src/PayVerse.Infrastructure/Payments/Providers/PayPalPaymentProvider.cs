using Microsoft.Extensions.Logging;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Payments;
using PayVerse.Domain.Bridges;
using PayVerse.Domain.Enums.Payments;

namespace PayVerse.Infrastructure.Payments.Providers;

public class PayPalPaymentProvider(PayPalHttpClient payPalClient,
                                   ILogger<PayPalPaymentProvider> logger) : IPaymentProvider
{
    public async Task<string> ProcessPaymentAsync(Guid paymentId,
                                                  decimal amount,
                                                  string currency,
                                                  IDictionary<string, string> paymentDetails)
    {
        try
        {
            logger.LogInformation("Processing payment {PaymentId} via PayPal", paymentId);

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE", // Changed from Intent to CheckoutPaymentIntent
                PurchaseUnits =
                [
                    new()
                    {
                        ReferenceId = paymentId.ToString(),
                        AmountWithBreakdown = new AmountWithBreakdown // Changed from Amount to AmountWithBreakdown
                        {
                            CurrencyCode = currency,
                            Value = amount.ToString("0.00")
                        }
                    }
                ],
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = paymentDetails.TryGetValue("returnUrl", out var returnUrl) ? returnUrl : null,
                    CancelUrl = paymentDetails.TryGetValue("cancelUrl", out var cancelUrl) ? cancelUrl : null
                }
            });

            var response = await payPalClient.Execute(request);
            var order = response.Result<Order>();

            return order.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing PayPal payment {PaymentId}", paymentId);
            throw new PaymentProcessingException("Failed to process payment through PayPal", ex);
        }
    }

    public async Task<bool> RefundPaymentAsync(string transactionId, decimal amount, string currency)
    {
        try
        {
            var request = new CapturesRefundRequest(transactionId);
            request.RequestBody(new RefundRequest
            {
                Amount = new PayPalCheckoutSdk.Payments.Money // Fully qualified Money class
                {
                    CurrencyCode = currency,
                    Value = amount.ToString("0.00")
                }
            });

            var response = await payPalClient.Execute(request);
            var refund = response.Result<PayPalCheckoutSdk.Payments.Refund>(); // Fully qualified Refund class

            return refund.Status == "COMPLETED";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error refunding PayPal payment {TransactionId}", transactionId);
            return false;
        }
    }

    public async Task<PaymentStatus> CheckPaymentStatusAsync(string transactionId)
    {
        try
        {
            var request = new OrdersGetRequest(transactionId);
            var response = await payPalClient.Execute(request);
            var order = response.Result<Order>();

            return order.Status switch
            {
                "COMPLETED" => PaymentStatus.Processed,
                "APPROVED" => PaymentStatus.Processing,
                "CREATED" => PaymentStatus.Pending,
                "SAVED" => PaymentStatus.Pending,
                "PAYER_ACTION_REQUIRED" => PaymentStatus.Pending,
                "VOIDED" => PaymentStatus.Cancelled,
                _ => PaymentStatus.Unknown
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error checking PayPal payment status {TransactionId}", transactionId);
            return PaymentStatus.Unknown;
        }
    }

    public bool SupportsRecurringPayments() => true;

    public bool SupportsPartialRefunds() => true;

    public string GetProviderName() => "PayPal";
}

// Add this custom exception class
public class PaymentProcessingException : Exception
{
    public PaymentProcessingException(string message) : base(message)
    {
    }

    public PaymentProcessingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
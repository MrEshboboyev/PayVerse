using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using System.Net;

namespace PayVerse.Infrastructure.Adapters;

public class PayPalAdapter : IPaymentGateway
{
    private readonly PayPalHttpClient _client;

    public PayPalAdapter(string clientId, string clientSecret)
    {
        var environment = new SandboxEnvironment(clientId, clientSecret); // Use LiveEnvironment for production
        _client = new PayPalHttpClient(environment);
    }

    public async Task<Result> ProcessPaymentAsync(Payment payment)
    {
        try
        {
            var order = new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits =
                [
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "usd",
                            Value = payment.Amount.Value.ToString("F2")
                        },
                        Description = "Payment for invoice"
                    }
                ]
            };

            var request = new OrdersCreateRequest();
            request.RequestBody(order);

            var response = await _client.Execute(request);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                var result = response.Result<Order>();

                // Capture the payment
                var captureRequest = new OrdersCaptureRequest(result.Id);
                var captureResponse = await _client.Execute(captureRequest);

                if (captureResponse.StatusCode == HttpStatusCode.Created)
                {
                    return Result.Success();
                }
            }

            return Result.Failure(
                DomainErrors.PayPal.ProcessingFailed("Payment processing failed."));
        }
        catch (Exception ex)
        {
            // Log the exception (logging code not shown here)
            Console.WriteLine($"Payment processing failed: {ex.Message}");
            return Result.Failure(
                DomainErrors.PayPal.ProcessingFailed(ex.Message));
        }
    }
}

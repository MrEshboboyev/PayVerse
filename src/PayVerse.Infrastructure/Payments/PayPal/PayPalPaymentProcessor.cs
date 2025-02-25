using Microsoft.Extensions.Options;
using PayVerse.Domain.Abstractions.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using PayVerse.Infrastructure.Payments.PayPal.Models;
using System.Net.Http.Json;

namespace PayVerse.Infrastructure.Payments.PayPal;

/// <summary>
/// Implements the payment processor for PayPal.
/// </summary>
internal sealed class PayPalPaymentProcessor(
    HttpClient httpClient, 
    IOptions<PayPalSettings> settings) : IPaymentProcessor
{
    private readonly PayPalSettings _settings = settings.Value;
    private string _accessToken;
    private DateTime _tokenExpiry;

    public async Task<Result> ProcessPaymentAsync(Payment payment)
    {
        try
        {
            await EnsureAccessTokenAsync();

            var request = new PayPalPaymentRequest
            {
                Intent = "CAPTURE",
                PurchaseUnits =
                [
                    new PurchaseUnit
                    {
                        Amount = new PayPalAmount
                        {
                            CurrencyCode = "USD",
                            Value = payment.Amount.Value.ToString("F2")
                        },
                        ReferenceId = payment.Id.ToString()
                    }
                ]
            };

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await httpClient.PostAsJsonAsync("v2/checkout/orders", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result.Failure(
                    DomainErrors.PayPal.PaymentProcessingFailed(errorContent));
            }

            var paypalResponse = await response.Content.ReadFromJsonAsync<PayPalPaymentResponse>();

            if (paypalResponse.Status == "CREATED" || paypalResponse.Status == "APPROVED")
            {
                // Payment is successfully initiated but not yet completed
                // In a real implementation, we'd store the PayPal order ID
                return Result.Success();
            }

            return Result.Failure(
                DomainErrors.PayPal.PaymentFailedWithStatus(paypalResponse.Status));
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.PayPal.PaymentProcessingException(ex.Message));
        }
    }

    public async Task<Result> ValidatePaymentAsync(Payment payment)
    {
        // Validate payment amount, currency, etc.
        if (payment.Amount.Value <= 0)
        {
            return Result.Failure(
                DomainErrors.PayPal.PaymentAmountMustBeGreaterThanZero);
        }

        return Result.Success();
    }

    public async Task<Result> CancelPaymentAsync(Payment payment)
    {
        try
        {
            await EnsureAccessTokenAsync();

            // In a real implementation, we would use the stored PayPal order ID
            // For demonstration, we'll assume we have an orderId from somewhere
            string orderId = "MOCK_ORDER_ID_" + payment.Id;

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await httpClient.PostAsync($"v2/checkout/orders/{orderId}/cancel", null);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result.Failure(
                    DomainErrors.PayPal.PaymentCancellationFailed(errorContent));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.PayPal.PaymentCancellationException(ex.Message));
        }
    }

    public async Task<Result> RefundPaymentAsync(Payment payment)
    {
        try
        {
            await EnsureAccessTokenAsync();

            // In a real implementation, we would use the stored PayPal capture ID
            // For demonstration, we'll assume we have a captureId from somewhere
            string captureId = "MOCK_CAPTURE_ID_" + payment.Id;

            var request = new PayPalRefundRequest
            {
                Amount = new PayPalAmount
                {
                    CurrencyCode = "USD",
                    Value = payment.Amount.Value.ToString("F2")
                },
                NoteToPayer = "Refund for order"
            };

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await httpClient.PostAsJsonAsync($"v2/payments/captures/{captureId}/refund", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result.Failure(
                    DomainErrors.PayPal.RefundFailed(errorContent));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                DomainErrors.PayPal.RefundException(ex.Message));
        }
    }

    #region Private Methods

    private async Task EnsureAccessTokenAsync()
    {
        if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _tokenExpiry)
        {
            var authString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}"));

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await httpClient.PostAsync("v1/oauth2/token", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to obtain PayPal access token");
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<PayPalTokenResponse>();
            _accessToken = tokenResponse.AccessToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60); // Buffer of 60 seconds
        }
    }

    #endregion
}

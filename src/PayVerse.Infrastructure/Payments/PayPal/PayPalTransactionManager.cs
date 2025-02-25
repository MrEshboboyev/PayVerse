using Microsoft.Extensions.Options;
using PayVerse.Domain.Abstractions.Payments;
using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using PayVerse.Infrastructure.Payments.PayPal.Models;
using System.Net.Http.Json;

namespace PayVerse.Infrastructure.Payments.PayPal;

/// <summary>
/// Implements the transaction manager for PayPal.
/// </summary>
internal sealed class PayPalTransactionManager(
    HttpClient httpClient, 
    IOptions<PayPalSettings> settings) : IPaymentTransaction
{
    private readonly PayPalSettings _settings = settings.Value;
    private string _accessToken;
    private DateTime _tokenExpiry;

    public string TransactionId { get; private set; } = 
        string.Concat("PAYPAL-", Guid.NewGuid().ToString("N").AsSpan(0, 10));

    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    public async Task<Result<string>> CreateTransactionAsync(Payment payment)
    {
        try
        {
            await EnsureAccessTokenAsync();

            // In a real implementation, we would create a proper transaction record
            // For demonstration, we'll just return a mock transaction ID
            TransactionId = string.Concat("PAYPAL-", Guid.NewGuid().ToString("N").AsSpan(0, 10));
            Timestamp = DateTime.UtcNow;

            return Result.Success(TransactionId);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(
                DomainErrors.PayPal.FailedToCreateTransaction(ex.Message));
        }
    }

    public async Task<Result<string>> GetTransactionStatusAsync(string transactionId)
    {
        try
        {
            await EnsureAccessTokenAsync();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            // In a real implementation, we would make an API call to PayPal to get transaction status
            // For demonstration, we'll just return a mock status
            return Result.Success("COMPLETED");
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(
                DomainErrors.PayPal.FailedToGetTransactionStatus(ex.Message));
        }
    }

    public async Task<Result<string>> GenerateReceiptAsync(string transactionId)
    {
        try
        {
            // In a real implementation, we would generate a proper receipt with transaction details
            // For demonstration, we'll just return a mock receipt
            var receipt = $@"
            PayPal Receipt
            ------------------------------
            Transaction ID: {transactionId}
            Date: {Timestamp}
            Status: COMPLETED
            ------------------------------
            Thank you for your payment!
            ";

            return Result.Success(receipt);
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(
                DomainErrors.PayPal.FailedToGenerateReceipt(ex.Message));
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
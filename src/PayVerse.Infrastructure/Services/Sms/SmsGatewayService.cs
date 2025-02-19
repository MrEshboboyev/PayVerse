using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayVerse.Application.Interfaces;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using System.Text;

namespace PayVerse.Infrastructure.Services.Sms;

public class SmsGatewayService(
    IConfiguration configuration,
    ILogger<SmsGatewayService> logger) : ISmsService
{
    private readonly string _apiKey = configuration["SmsGateway:ApiKey"]
        ?? throw new ArgumentException("SMS Gateway API Key is not configured.");
    private readonly string _apiUrl = configuration["SmsGateway:ApiUrl"]
        ?? throw new ArgumentException("SMS Gateway API URL is not configured.");
    private readonly ILogger<SmsGatewayService> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result> SendSms(string phoneNumber, string message)
    {
        try
        {
            using var client = new HttpClient();
            var content = new StringContent(
                $"api_key={_apiKey}" +
                $"&number={Uri.EscapeDataString(phoneNumber)}" +
                $"&message={Uri.EscapeDataString(message)}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            var response = await client.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"SMS sent to {phoneNumber}");
                return Result.Success();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _logger.LogWarning($"Failed to send SMS to {phoneNumber}. Response: {errorMessage}");
                return Result.Failure(DomainErrors.Sms.GatewaySendFailed($"{response.StatusCode} - {errorMessage}"));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send SMS to {phoneNumber}");
            return Result.Failure(
                DomainErrors.Sms.GatewaySendFailed(ex.Message));
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayVerse.Application.Interfaces;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PayVerse.Infrastructure.Services.Sms;

public class TwilioSmsService(
    IConfiguration configuration,
    ILogger<TwilioSmsService> logger) : ISmsService
{
    private readonly ILogger<TwilioSmsService> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly string _accountSid =
        configuration["Twilio:AccountSid"] ?? throw new ArgumentException("Twilio AccountSid is not configured.");
    private readonly string _authToken =
        configuration["Twilio:AuthToken"] ?? throw new ArgumentException("Twilio AuthToken is not configured.");
    private readonly PhoneNumber _twilioNumber =
        new(configuration["Twilio:PhoneNumber"] ?? throw new ArgumentException("Twilio PhoneNumber is not configured."));

    public async Task<Result> SendSms(string phoneNumber, string message)
    {
        try
        {
            TwilioClient.Init(_accountSid, _authToken);

            var result = await MessageResource.CreateAsync(
                body: message,
                from: _twilioNumber,
                to: new PhoneNumber(phoneNumber)
            );

            _logger.LogInformation($"SMS sent to {phoneNumber}. SID: {result.Sid}");

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send SMS to {phoneNumber}");
            return Result.Failure(
                DomainErrors.Sms.TwilioSendFailed(phoneNumber, ex.Message));
        }
    }
}

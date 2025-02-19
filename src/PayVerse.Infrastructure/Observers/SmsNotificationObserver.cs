using PayVerse.Domain.Observers;
using Microsoft.Extensions.Logging;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Application.Interfaces;

namespace PayVerse.Infrastructure.Observers;

public class SmsNotificationObserver(
    ILogger<SmsNotificationObserver> logger, 
    ISmsService smsService) : ITransactionObserver
{
    private readonly ILogger<SmsNotificationObserver> _logger = logger;
    private readonly ISmsService _smsService = smsService;

    public async Task OnTransactionProcessedAsync(Transaction transaction)
    {
        string message = $"Transaction {transaction.Id} of ${transaction.Amount.Value} processed.";

        try
        {
            await _smsService.SendSms("1234567890", message);
            _logger.LogInformation($"SMS notification sent for transaction {transaction.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send SMS for transaction {transaction.Id}");
        }
    }
}
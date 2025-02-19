using PayVerse.Domain.Observers;
using Microsoft.Extensions.Logging;
using PayVerse.Application.Interfaces;
using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Infrastructure.Observers;

public class EmailNotificationObserver(
    ILogger<EmailNotificationObserver> logger, 
    IEmailService emailService) : ITransactionObserver
{
    private readonly ILogger<EmailNotificationObserver> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    public async Task OnTransactionProcessedAsync(Transaction transaction)
    {
        string subject = $"Transaction Processed - {transaction.Id}";
        string body = $"Transaction {transaction.Id} of ${transaction.Amount.Value} has been processed.";

        try
        {
            await _emailService.SendEmail("user@example.com", subject, body);
            _logger.LogInformation($"Email notification sent for transaction {transaction.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email for transaction {transaction.Id}");
        }
    }
}
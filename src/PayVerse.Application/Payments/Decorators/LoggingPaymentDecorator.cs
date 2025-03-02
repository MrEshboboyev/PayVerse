using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Entities.Payments;

namespace PayVerse.Application.Payments.Decorators;

/// <summary>
/// Adds logging capabilities to payment processing
/// </summary>
public class LoggingPaymentDecorator(
    IPaymentProcessor paymentProcessor,
    ILogger<LoggingPaymentDecorator> logger,
    IAuditLogService auditLogService,
    ICurrentUserService currentUserService) : PaymentProcessorDecorator(paymentProcessor)
{
    public override async Task<PaymentResult> ProcessPaymentAsync(Payment payment,
                                                                  CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting payment processing for payment ID: {PaymentId}", payment.Id);

        try
        {
            var result = await base.ProcessPaymentAsync(payment, cancellationToken);

            // Log success
            logger.LogInformation("Payment processed successfully. Transaction ID: {TransactionId}", result.TransactionId);

            // Create audit log entry
            await auditLogService.CreateAuditLogAsync(
                currentUserService.UserId,
                "PaymentProcessed",
                $"Payment {payment.Id} processed with transaction {result.TransactionId}",
                currentUserService.IpAddress,
                currentUserService.DeviceInfo,
                cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            // Log failure
            logger.LogError(ex, "Error processing payment for payment ID: {PaymentId}", payment.Id);

            // Create audit log for failure
            await auditLogService.CreateAuditLogAsync(
                currentUserService.UserId,
                "PaymentFailed",
                $"Payment {payment.Id} failed: {ex.Message}",
                currentUserService.IpAddress,
                currentUserService.DeviceInfo,
                cancellationToken);

            throw;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Common.Interfaces.Payments;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Application.Decorators.Payments;
using PayVerse.Application.Payments.Decorators;
using PayVerse.Domain.Decorators.Payments;

namespace PayVerse.Application.Payments.Factories;

public class PaymentProcessorFactory(IServiceProvider serviceProvider) : IPaymentProcessorFactory
{
    public IPaymentProcessor CreatePaymentProcessor(PaymentProcessorOptions options)
    {
        // Start with the base processor
        var processor = serviceProvider.GetRequiredService<PaymentProcessor>();
        IPaymentProcessor decorated = processor;

        // Apply decorators based on options
        if (options.EnableValidation)
        {
            var validationService = serviceProvider.GetRequiredService<IPaymentValidationService>();
            decorated = new ValidationPaymentDecorator(decorated, validationService);
        }

        if (options.EnableFraudDetection)
        {
            var fraudDetectionService = serviceProvider.GetRequiredService<IFraudDetectionService>();
            //var securityIncidentService = serviceProvider.GetRequiredService<ISecurityIncidentService>();
            decorated = new FraudDetectionPaymentDecorator(decorated, fraudDetectionService);
        }

        if (options.EnableLogging)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<LoggingPaymentDecorator>>();
            var auditLogService = serviceProvider.GetRequiredService<IAuditLogService>();
            var currentUserService = serviceProvider.GetRequiredService<ICurrentUserService>();
            decorated = new LoggingPaymentDecorator(decorated, logger, auditLogService, currentUserService);
        }

        if (options.EnableNotifications)
        {
            var notificationService = serviceProvider.GetRequiredService<INotificationService>();
            decorated = new NotificationPaymentDecorator(decorated, notificationService);
        }

        return decorated;
    }
}

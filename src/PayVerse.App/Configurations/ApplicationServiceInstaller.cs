using FluentValidation;
using MediatR;
using PayVerse.Application;
using PayVerse.Application.Behaviors;
using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Application.Decorators.Payments;
using PayVerse.Application.Payments.Decorators;
using PayVerse.Application.Security.Commands.TransferFundsWithDecorators;
using PayVerse.Application.Security.Decorators;
using PayVerse.Domain.Decorators.Payments;
using PayVerse.Domain.Decorators.Security;
using PayVerse.Infrastructure.Idempotence;

namespace PayVerse.App.Configurations;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR services for handling commands and queries
        services.AddMediatR(cfg =>
        {
            // Register handlers from the specified assembly
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        services.AddValidatorsFromAssembly(
            AssemblyReference.Assembly,
            includeInternalTypes: true);

        #region Payment Processing

        // Register the base component
        services.AddScoped<IPaymentProcessor, PaymentProcessor>();

        // Register decorators
        services.Decorate<IPaymentProcessor, ValidationPaymentDecorator>();
        services.Decorate<IPaymentProcessor, FraudDetectionPaymentDecorator>();
        services.Decorate<IPaymentProcessor, LoggingPaymentDecorator>();
        services.Decorate<IPaymentProcessor, NotificationPaymentDecorator>();

        #endregion

        #region Secure operations

        // Register transfer funds operation
        services.AddScoped<ISecureOperation<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>, TransferFundsWirthDecoratorsOperation>();

        // Decorate with security layers
        services.Decorate<ISecureOperation<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>>(
            (inner, provider) => new AuthenticationDecorator<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>(
                inner,
                provider.GetRequiredService<ICurrentUserService>(),
                provider.GetRequiredService<IAuthenticationService>()));

        services.Decorate<ISecureOperation<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>>(
            (inner, provider) => new AuthorizationDecorator<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>(
                inner,
                provider.GetRequiredService<ICurrentUserService>(),
                provider.GetRequiredService<IAuthorizationService>(),
                "VirtualAccount.Transfer"));

        services.Decorate<ISecureOperation<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>>(
            (inner, provider) => new RateLimitingDecorator<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>(
                inner,
                provider.GetRequiredService<ICurrentUserService>(),
                provider.GetRequiredService<IRateLimitingService>(),
                "TransferFunds",
                10, // Max 10 attempts
                TimeSpan.FromMinutes(5))); // Within 5 minutes

        services.Decorate<ISecureOperation<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>>(
            (inner, provider) => new IpFilteringDecorator<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>(
                inner,
                provider.GetRequiredService<ICurrentUserService>(),
                provider.GetRequiredService<IIpFilteringService>()));

        #endregion
    }
}
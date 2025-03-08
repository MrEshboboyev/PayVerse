using PayPalCheckoutSdk.Core;
using PayVerse.Application.Bridges;
using PayVerse.Application.Reports.Services;
using PayVerse.Application.Wallets.Converters;
using PayVerse.Domain.Adapters.Payments;
using PayVerse.Domain.Bridges;
using PayVerse.Domain.Observers;
using PayVerse.Domain.Observers.Reports;
using PayVerse.Domain.Observers.VirtualAccounts;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Strategies.Factories;
using PayVerse.Domain.Strategies.Payments;
using PayVerse.Infrastructure.Converters;
using PayVerse.Infrastructure.PaymentProviders.Adapters;
using PayVerse.Infrastructure.PaymentProviders.Adapters.PayPal;
using PayVerse.Infrastructure.PaymentProviders.Adapters.Stripe;
using PayVerse.Infrastructure.Payments.Providers;
using PayVerse.Infrastructure.Reports.Factories;
using PayVerse.Infrastructure.Reports.Generators;
using PayVerse.Infrastructure.Services.Observers;
using PayVerse.Infrastructure.Services.Security;
using Scrutor;
using Stripe;

namespace PayVerse.App.Configurations;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
              .Scan(
                  selector => selector
                      .FromAssemblies(
                         Infrastructure.AssemblyReference.Assembly,
                         Persistence.AssemblyReference.Assembly)
                      .AddClasses(false)
                      .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                      .AsMatchingInterface()
                      .WithScopedLifetime());

        services.AddHttpClient<ICurrencyConverter, CurrencyConverter>();
        services.AddScoped<IReportGeneratorFactory, ReportGeneratorFactory>();

        services.AddTransient<CsvReportGenerator>();
        services.AddTransient<ExcelReportGenerator>();
        services.AddTransient<HtmlReportGenerator>();
        services.AddTransient<JsonReportGenerator>();
        services.AddTransient<PdfReportGenerator>();
        services.AddTransient<TxtReportGenerator>();

        #region Payment Provider Configurations

        // Register the underlying payment services
        services.AddSingleton<StripePaymentService>();
        services.AddSingleton<PayPalService>();

        // Get provider from configuration
        var defaultProvider = configuration.GetValue<string>("PaymentSettings:DefaultProvider");

        // Register the appropriate adapter based on configuration
        switch (defaultProvider?.ToLower())
        {
            case "paypal":
                services.AddScoped<IPaymentGatewayAdapter, PayPalPaymentAdapter>();
                break;
            case "stripe":
            default:
                services.AddScoped<IPaymentGatewayAdapter, StripePaymentAdapter>();
                break;
        }

        // Optionally, you could register named instances for different scenarios
        services.AddKeyedScoped<IPaymentGatewayAdapter, StripePaymentAdapter>("stripe");
        services.AddKeyedScoped<IPaymentGatewayAdapter, PayPalPaymentAdapter>("paypal");

        #endregion

        #region Bridge Payment Provider Configurations

        // Register payment providers
        services.AddTransient<IPaymentProvider, StripePaymentProvider>(sp =>
        {
            var stripeClient = new StripeClient(configuration["Payments:Stripe:ApiKey"]);
            var logger = sp.GetRequiredService<ILogger<StripePaymentProvider>>();
            return new StripePaymentProvider(stripeClient, logger);
        });

        services.AddTransient<IPaymentProvider, PayPalPaymentProvider>(sp =>
        {
            // Change from IPayPalClient to PayPalHttpClient
            var payPalClient = new PayPalHttpClient(new SandboxEnvironment(
                configuration["Payments:PayPal:ClientId"],
                configuration["Payments:PayPal:ClientSecret"]));
            var logger = sp.GetRequiredService<ILogger<PayPalPaymentProvider>>();
            return new PayPalPaymentProvider(payPalClient, logger);
        });

        // Register payment processors
        services.AddTransient<StandardPaymentProcessor>();

        // Register RecurringPaymentProcessor with ILoggerFactory
        services.AddTransient(sp =>
        {
            var paymentProvider = sp.GetRequiredService<IPaymentProvider>();
            var paymentRepository = sp.GetRequiredService<IPaymentRepository>();
            var logger = sp.GetRequiredService<ILogger<RecurringPaymentProcessor>>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            return new RecurringPaymentProcessor(
                paymentProvider,
                paymentRepository,
                logger,
                loggerFactory);
        });

        // Register a factory for creating appropriate payment processor based on payment type
        services.AddTransient<Func<PaymentProcessorType, IPaymentProvider, PaymentProcessor>>(sp => (type, provider) =>
        {
            return type switch
            {
                PaymentProcessorType.Standard => sp.GetRequiredService<StandardPaymentProcessor>(),
                PaymentProcessorType.Recurring => sp.GetRequiredService<RecurringPaymentProcessor>(),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        });

        #endregion

        #region Observers Configuration

        // Register the observers as singletons
        services.AddSingleton<IObserver, VirtualAccountBalanceObserver>();
        services.AddSingleton<IObserver, FinancialReportObserver>();

        // Add a payment observer registration service
        services.AddScoped<IPaymentObserverRegistrationService, PaymentObserverRegistrationService>();

        #endregion

        #region Strategy Configuration

        // Register all strategies
        services.AddScoped<WalletPaymentStrategy>();

        // Register the factory
        services.AddScoped<IPaymentProcessingStrategyFactory, PaymentProcessingStrategyFactory>();

        // Register the processor
        services.AddScoped<PaymentProcessor>();

        #endregion
    }
}
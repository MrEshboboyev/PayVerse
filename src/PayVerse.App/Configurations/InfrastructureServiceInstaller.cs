using PayVerse.Application.Reports.Services;
using PayVerse.Application.Wallets.Converters;
using PayVerse.Infrastructure.Converters;
using PayVerse.Infrastructure.PaymentProviders.Adapters;
using PayVerse.Infrastructure.PaymentProviders.Adapters.PayPal;
using PayVerse.Infrastructure.PaymentProviders.Adapters.Stripe;
using PayVerse.Infrastructure.Reports.Factories;
using PayVerse.Infrastructure.Reports.Generators;
using PayVerse.Infrastructure.Services.Security;
using Scrutor;

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

        services.AddSingleton<AuditLogService>(AuditLogService.Instance);

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
                services.AddScoped<IPaymentProcessor, PayPalPaymentAdapter>();
                break;
            case "stripe":
            default:
                services.AddScoped<IPaymentProcessor, StripePaymentAdapter>();
                break;
        }

        // Optionally, you could register named instances for different scenarios
        services.AddKeyedScoped<IPaymentProcessor, StripePaymentAdapter>("stripe");
        services.AddKeyedScoped<IPaymentProcessor, PayPalPaymentAdapter>("paypal");

        #endregion
    }
}
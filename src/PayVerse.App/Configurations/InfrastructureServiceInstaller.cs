using PayVerse.Application.Reports.Services;
using PayVerse.Application.Wallets.Converters;
using PayVerse.Infrastructure.Converters;
using PayVerse.Infrastructure.Reports.Factories;
using PayVerse.Infrastructure.Reports.Generators;
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
        
        services.AddHttpClient<ICurrencyConverter, CurrencyConverter>();
        // services.AddScoped<IReportGenerator, ReportGene>(); // Register the report generator
        services.AddScoped<IReportGeneratorFactory, ReportGeneratorFactory>();

        
        services.AddTransient<CsvReportGenerator>();
        services.AddTransient<ExcelReportGenerator>();
        services.AddTransient<HtmlReportGenerator>();
        services.AddTransient<JsonReportGenerator>();
        services.AddTransient<PdfReportGenerator>();
        services.AddTransient<TxtReportGenerator>();
    }
}
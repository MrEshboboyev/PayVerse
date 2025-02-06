using FluentValidation;
using PayVerse.Application.Behaviors;
using PayVerse.Infrastructure.Idempotence;
using MediatR;
using PayVerse.Application;

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
    }
}
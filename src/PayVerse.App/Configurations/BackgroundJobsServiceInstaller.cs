using PayVerse.Infrastructure.BackgroundJobs;
using Quartz;

namespace PayVerse.App.Configurations;

public class BackgroundJobsServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Register the jobs with DI
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        // Configure Quartz
        services.AddQuartz(configure =>
        {
            // Configure ProcessOutboxMessagesJob
            var processOutboxJobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            configure
                .AddJob<ProcessOutboxMessagesJob>(processOutboxJobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(processOutboxJobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(10)
                        .RepeatForever()));
        });

        // Add Quartz as a hosted service
        services.AddQuartzHostedService();
    }
}
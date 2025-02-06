using PayVerse.Domain.Primitives;
using PayVerse.Persistence.Outbox;
using PayVerse.Persistence;
using MediatR;
using Newtonsoft.Json;
using Quartz;
using Microsoft.EntityFrameworkCore;
using Polly.Retry;
using Polly;

namespace PayVerse.Infrastructure.BackgroundJobs;

/// <summary> 
/// Background job for processing outbox messages. This job retrieves unprocessed outbox messages, 
/// deserializes the domain events, publishes them, and marks the messages as processed. 
/// </summary>
[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(
    ApplicationDbContext dbContext,
    IPublisher publisher) : IJob
{
    /// <summary> 
    /// Executes the job to process outbox messages. 
    /// </summary> 
    /// <param name="context">The job execution context.</param>
    public async Task Execute(IJobExecutionContext context)
    {
        // Retrieve unprocessed outbox messages
        var messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        // Process each outbox message
        foreach (var outboxMessage in messages)
        {
            // Deserialize the domain event from the message content
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings 
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                continue;
            }

            // Define a retry policy to handle transient exceptions
            var policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(50 * attempt));

            // Execute the publish operation with retry policy
            var result = await policy.ExecuteAndCaptureAsync(() =>
                publisher.Publish(
                    domainEvent,
                    context.CancellationToken));

            // Record any errors that occurred during publishing
            outboxMessage.Error = result.FinalException?.ToString();

            // Mark the outbox message as processed
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        // Save changes to the database
        await dbContext.SaveChangesAsync();
    }
}
using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Application.Decorators.Security;
using PayVerse.Domain.Decorators.Security;

namespace PayVerse.Application.Security.Decorators;

/// <summary>
/// Adds rate limiting to operations
/// </summary>
public class RateLimitingDecorator<TRequest, TResult>(
    ISecureOperation<TRequest, TResult> decoratedOperation,
    ICurrentUserService currentUserService,
    IRateLimitingService rateLimitingService,
    string operationKey
    //,
    //int maxAttempts,
    //TimeSpan timeWindow
    ) : SecureOperationDecorator<TRequest, TResult>(decoratedOperation)
{
    public override async Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        // Check rate limit
        var userId = currentUserService.UserId;
        var ipAddress = currentUserService.IpAddress;

        //var isRateLimited = await rateLimitingService.IsRateLimitedAsync(
        //    userId,
        //    ipAddress,
        //    operationKey,
        //    maxAttempts,
        //    timeWindow,
        //    cancellationToken);

        //if (isRateLimited)
        //{
        //    throw new RateLimitExceededException($"Rate limit exceeded for operation {operationKey}. Please try again later.");
        //}

        var isRateLimited = await rateLimitingService.IsAllowedAsync(
            userId.ToString(),
            "none",
            cancellationToken);

        if (isRateLimited)
        {
            throw new RateLimitExceededException($"Rate limit exceeded for operation {operationKey}. Please try again later.");
        }

        //// Increment attempt counter
        //await rateLimitingService.RecordAttemptAsync(
        //    userId,
        //    ipAddress,
        //    operationKey,
        //    cancellationToken);

        await rateLimitingService.RecordRequestAsync(ipAddress, "none", cancellationToken);

        return await base.ExecuteAsync(request, cancellationToken);
    }
}

public class RateLimitExceededException(string message) : Exception(message)
{
}
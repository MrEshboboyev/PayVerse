using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PayVerse.Application.Common.Interfaces.Security;
using System.Text.Json;

namespace PayVerse.Infrastructure.Services.Security;

/// <summary>
/// Implementation of rate limiting service using distributed cache
/// </summary>
public class RateLimitingService(
    IDistributedCache cache,
    IOptions<RateLimitingOptions> options,
    ILogger<RateLimitingService> logger) : IRateLimitingService
{
    private readonly RateLimitingOptions _options = options.Value;

    public async Task<bool> IsAllowedAsync(string clientId, string endpoint, CancellationToken cancellationToken = default)
    {
        var limits = GetLimitsForEndpoint(endpoint);
        var key = GenerateCacheKey(clientId, endpoint);

        var rateData = await GetRateLimitDataAsync(key, cancellationToken);

        // If no data exists, the client is allowed
        if (rateData == null)
            return true;

        // Check if the client has exceeded the limit
        if (rateData.RequestCount >= limits.MaxRequests)
        {
            var now = DateTime.UtcNow;
            var windowEnd = rateData.WindowStart.AddSeconds(limits.WindowInSeconds);

            // If still within time window and count exceeded, block the request
            if (now < windowEnd)
            {
                logger.LogWarning("Rate limit exceeded for client {ClientId} on endpoint {Endpoint}", clientId, endpoint);
                return false;
            }

            // Time window has passed, so client is allowed again
            return true;
        }

        return true;
    }

    public async Task RecordRequestAsync(string clientId, string endpoint, CancellationToken cancellationToken = default)
    {
        var limits = GetLimitsForEndpoint(endpoint);
        var key = GenerateCacheKey(clientId, endpoint);

        var rateData = await GetRateLimitDataAsync(key, cancellationToken);
        var now = DateTime.UtcNow;

        if (rateData == null)
        {
            // New rate limit window
            rateData = new RateLimitData
            {
                WindowStart = now,
                RequestCount = 1
            };
        }
        else
        {
            var windowEnd = rateData.WindowStart.AddSeconds(limits.WindowInSeconds);

            if (now > windowEnd)
            {
                // Start a new window
                rateData.WindowStart = now;
                rateData.RequestCount = 1;
            }
            else
            {
                // Increment request count in current window
                rateData.RequestCount++;
            }
        }

        // Save updated data to cache
        await cache.SetStringAsync(
            key,
            JsonSerializer.Serialize(rateData),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(limits.WindowInSeconds * 2)
            },
            cancellationToken);
    }

    public async Task<int> GetRemainingRequestsAsync(string clientId, string endpoint, CancellationToken cancellationToken = default)
    {
        var limits = GetLimitsForEndpoint(endpoint);
        var key = GenerateCacheKey(clientId, endpoint);

        var rateData = await GetRateLimitDataAsync(key, cancellationToken);

        if (rateData == null)
            return limits.MaxRequests;

        var now = DateTime.UtcNow;
        var windowEnd = rateData.WindowStart.AddSeconds(limits.WindowInSeconds);

        // If window has expired, return full limit
        if (now > windowEnd)
            return limits.MaxRequests;

        return Math.Max(0, limits.MaxRequests - rateData.RequestCount);
    }

    private async Task<RateLimitData> GetRateLimitDataAsync(string key, CancellationToken cancellationToken)
    {
        var cachedData = await cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
            return null;

        try
        {
            return JsonSerializer.Deserialize<RateLimitData>(cachedData);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Error deserializing rate limit data for key {Key}", key);
            return null;
        }
    }

    private string GenerateCacheKey(string clientId, string endpoint)
    {
        return $"RateLimit:{clientId}:{endpoint}";
    }

    private (int MaxRequests, int WindowInSeconds) GetLimitsForEndpoint(string endpoint)
    {
        // Check if there are specific limits for this endpoint
        if (_options.EndpointLimits.TryGetValue(endpoint, out var limits))
            return limits;

        // Otherwise use the default limits
        return _options.DefaultLimits;
    }

    private class RateLimitData
    {
        public DateTime WindowStart { get; set; }
        public int RequestCount { get; set; }
    }
}

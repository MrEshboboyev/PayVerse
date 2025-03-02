namespace PayVerse.Infrastructure.Services.Security;

/// <summary>
/// Configuration options for rate limiting
/// </summary>
public class RateLimitingOptions
{
    public (int MaxRequests, int WindowInSeconds) DefaultLimits { get; set; } = (100, 60);
    public Dictionary<string, (int MaxRequests, int WindowInSeconds)> EndpointLimits { get; set; } = new();
}

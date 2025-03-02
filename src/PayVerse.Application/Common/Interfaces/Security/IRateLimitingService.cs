namespace PayVerse.Application.Common.Interfaces.Security;

/// <summary>
/// Service for applying rate limiting to API endpoints and user actions
/// </summary>
public interface IRateLimitingService
{
    /// <summary>
    /// Checks if a request from the given client should be allowed or blocked due to rate limiting
    /// </summary>
    /// <param name="clientId">Identifier for the client (IP, API key, user ID, etc.)</param>
    /// <param name="endpoint">The API endpoint or action being accessed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the request is allowed, false if it exceeds rate limits</returns>
    Task<bool> IsAllowedAsync(string clientId, string endpoint, CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a request from the client to update rate limiting counters
    /// </summary>
    /// <param name="clientId">Identifier for the client</param>
    /// <param name="endpoint">The API endpoint or action being accessed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task RecordRequestAsync(string clientId, string endpoint, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets remaining requests allowed for the client within the current time window
    /// </summary>
    /// <param name="clientId">Identifier for the client</param>
    /// <param name="endpoint">The API endpoint or action being accessed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of remaining requests allowed</returns>
    Task<int> GetRemainingRequestsAsync(string clientId, string endpoint, CancellationToken cancellationToken = default);
}

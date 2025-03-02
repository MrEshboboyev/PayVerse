namespace PayVerse.Application.Common.Interfaces.Security;

/// <summary>
/// Service for IP address filtering and validation
/// </summary>
public interface IIpFilteringService
{
    /// <summary>
    /// Checks if an IP address is blocked
    /// </summary>
    /// <param name="ipAddress">IP address to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the IP address is blocked, false otherwise</returns>
    Task<bool> IsIpBlockedAsync(string ipAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Blocks an IP address
    /// </summary>
    /// <param name="ipAddress">IP address to block</param>
    /// <param name="reason">Reason for blocking the IP address</param>
    /// <param name="expiryDate">Date when the block expires (null for permanent block)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the IP address was successfully blocked</returns>
    Task<bool> BlockIpAsync(string ipAddress, string reason, DateTime? expiryDate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unblocks an IP address
    /// </summary>
    /// <param name="ipAddress">IP address to unblock</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the IP address was successfully unblocked</returns>
    Task<bool> UnblockIpAsync(string ipAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of blocked IP addresses
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of blocked IP addresses</returns>
    Task<IEnumerable<string>> GetBlockedIpsAsync(CancellationToken cancellationToken = default);
}
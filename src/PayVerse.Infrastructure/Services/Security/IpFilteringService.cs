using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Repositories.Security;

namespace PayVerse.Infrastructure.Services.Security;

/// <summary>
/// Implementation of IP filtering service
/// </summary>
public class IpFilteringService(
    IBlockedIpRepository blockedIpRepository,
    ILogger<IpFilteringService> logger) : IIpFilteringService
{
    public async Task<bool> IsIpBlockedAsync(
        string ipAddress,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            logger.LogWarning("Attempt to check null or empty IP address");
            return false;
        }

        var blockedIp = await blockedIpRepository.GetByIpAddressAsync(ipAddress, cancellationToken);

        if (blockedIp == null)
        {
            return false;
        }

        // Check if the block has expired
        if (blockedIp.ExpiryDate.HasValue && blockedIp.ExpiryDate.Value < DateTime.UtcNow)
        {
            // Auto-remove expired blocks
            await blockedIpRepository.RemoveAsync(blockedIp.Id, cancellationToken);
            logger.LogInformation("Expired IP block for {IpAddress} automatically removed", ipAddress);
            return false;
        }

        return true;
    }

    public async Task<bool> BlockIpAsync(string ipAddress, string reason, DateTime? expiryDate = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            logger.LogWarning("Attempt to block null or empty IP address");
            return false;
        }

        // Check if already blocked
        if (await IsIpBlockedAsync(ipAddress, cancellationToken))
        {
            logger.LogInformation("IP address {IpAddress} is already blocked", ipAddress);
            return true;
        }

        // Use the domain factory method to create a BlockedIp entity
        var blockedIp = BlockedIp.Create(
            Guid.NewGuid(),
            ipAddress,
            reason,
            expiryDate);

        await blockedIpRepository.AddAsync(blockedIp, cancellationToken);

        logger.LogInformation("IP address {IpAddress} blocked. Reason: {Reason}, Expiry: {Expiry}",
            ipAddress, reason, expiryDate?.ToString() ?? "Never");

        return true;
    }

    public async Task<bool> UnblockIpAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            logger.LogWarning("Attempt to unblock null or empty IP address");
            return false;
        }

        var blockedIp = await blockedIpRepository.GetByIpAddressAsync(ipAddress, cancellationToken);

        if (blockedIp == null)
        {
            logger.LogInformation("IP address {IpAddress} is not blocked", ipAddress);
            return false;
        }

        await blockedIpRepository.RemoveAsync(blockedIp.Id, cancellationToken);

        logger.LogInformation("IP address {IpAddress} unblocked", ipAddress);

        return true;
    }

    public async Task<IEnumerable<string>> GetBlockedIpsAsync(CancellationToken cancellationToken = default)
    {
        var blockedIps = await blockedIpRepository.GetAllAsync(cancellationToken);
        var currentTime = DateTime.UtcNow;

        // Filter out expired blocks
        var activeBlockedIps = blockedIps
            .Where(b => !b.ExpiryDate.HasValue || b.ExpiryDate.Value > currentTime)
            .Select(b => b.IpAddress)
            .ToList();

        return activeBlockedIps;
    }
}
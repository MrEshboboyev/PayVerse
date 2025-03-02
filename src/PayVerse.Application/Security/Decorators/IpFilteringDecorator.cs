using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Application.Decorators.Security;
using PayVerse.Domain.Decorators.Security;
using PayVerse.Domain.Enums.Security;
using System.Security;

namespace PayVerse.Application.Security.Decorators;

/// <summary>
/// Adds IP filtering to operations
/// </summary>
public class IpFilteringDecorator<TRequest, TResult>(
    ISecureOperation<TRequest, TResult> decoratedOperation,
    ICurrentUserService currentUserService,
    IIpFilteringService ipFilteringService) : SecureOperationDecorator<TRequest, TResult>(decoratedOperation)
{
    public override async Task<TResult> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default)
    {
        var ipAddress = currentUserService.IpAddress;

        var isIpBlocked = await ipFilteringService.IsIpBlockedAsync(ipAddress, cancellationToken);

        if (isIpBlocked)
        {
            //// Log security incident
            //var securityIncidentService = new SecurityIncidentService(); // This would normally be injected
            //await securityIncidentService.LogIncidentAsync(
            //    SecurityIncidentType.BlockedIpAttempt,
            //    $"Blocked IP {ipAddress} attempted to access secure operation",
            //    currentUserService.UserId,
            //    cancellationToken);

            //throw new SecurityException($"Access denied from IP address {ipAddress}: {ipValidationResult.BlockReason}"); // fix this - coming soon
            throw new SecurityException($"Access denied from IP address {ipAddress}: none");
        }

        return await base.ExecuteAsync(request, cancellationToken);
    }
}

using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Domain.Entities.Security;
using System.Collections.Concurrent;

namespace PayVerse.Infrastructure.Services.Security;

public class AuditLogService(ILogger<AuditLogService> logger) : IAuditLogService
{
    private readonly ConcurrentDictionary<Guid, AuditLog> _auditLogs = new();

    public Task<Guid> CreateAuditLogAsync(
        Guid userId,
        string action,
        string details,
        string ipAddress,
        string deviceInfo,
        CancellationToken cancellationToken = default)
    {
        var auditLog = AuditLog.Record(
            Guid.NewGuid(),
            userId,
            action,
            details,
            ipAddress,
            deviceInfo);

        _auditLogs[auditLog.Id] = auditLog;
        logger.LogInformation("Audit log created: {AuditLogId}", auditLog.Id);

        return Task.FromResult(auditLog.Id);
    }

    public Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var logs = _auditLogs.Values
            .Where(log => log.UserId == userId &&
                          (!startDate.HasValue || log.Timestamp >= startDate) &&
                          (!endDate.HasValue || log.Timestamp <= endDate))
            .ToList();

        return Task.FromResult<IEnumerable<AuditLog>>(logs);
    }

    public Task<IEnumerable<AuditLog>> GetActionAuditLogsAsync(
        string action,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var logs = _auditLogs.Values
            .Where(log => log.Action.Equals(action, StringComparison.OrdinalIgnoreCase) &&
                          (!startDate.HasValue || log.Timestamp >= startDate) &&
                          (!endDate.HasValue || log.Timestamp <= endDate))
            .ToList();

        return Task.FromResult<IEnumerable<AuditLog>>(logs);
    }

    public Task<IEnumerable<AuditLog>> GetIpAddressAuditLogsAsync(
        string ipAddress,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var logs = _auditLogs.Values
            .Where(log => log.IpAddress == ipAddress &&
                          (!startDate.HasValue || log.Timestamp >= startDate) &&
                          (!endDate.HasValue || log.Timestamp <= endDate))
            .ToList();

        return Task.FromResult<IEnumerable<AuditLog>>(logs);
    }
}

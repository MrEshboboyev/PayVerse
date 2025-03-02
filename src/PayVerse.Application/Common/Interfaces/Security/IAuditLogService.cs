using PayVerse.Domain.Entities.Security;

namespace PayVerse.Application.Common.Interfaces.Security;

/// <summary>
/// Service for managing audit logs
/// </summary>
public interface IAuditLogService
{
    /// <summary>
    /// Creates a new audit log entry
    /// </summary>
    /// <param name="userId">ID of the user performing the action</param>
    /// <param name="action">Action being performed</param>
    /// <param name="details">Details about the action</param>
    /// <param name="ipAddress">IP address of the request</param>
    /// <param name="deviceInfo">Information about the device</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ID of the created audit log</returns>
    Task<Guid> CreateAuditLogAsync(
        Guid userId,
        string action,
        string details,
        string ipAddress,
        string deviceInfo,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs for a specific user
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <param name="startDate">Start date for filtering</param>
    /// <param name="endDate">End date for filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of audit logs</returns>
    Task<IEnumerable<AuditLog>> GetUserAuditLogsAsync(
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs for a specific action
    /// </summary>
    /// <param name="action">Action to filter by</param>
    /// <param name="startDate">Start date for filtering</param>
    /// <param name="endDate">End date for filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of audit logs</returns>
    Task<IEnumerable<AuditLog>> GetActionAuditLogsAsync(
        string action,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets audit logs for a specific IP address
    /// </summary>
    /// <param name="ipAddress">IP address to filter by</param>
    /// <param name="startDate">Start date for filtering</param>
    /// <param name="endDate">End date for filtering</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of audit logs</returns>
    Task<IEnumerable<AuditLog>> GetIpAddressAuditLogsAsync(
        string ipAddress,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default);
}
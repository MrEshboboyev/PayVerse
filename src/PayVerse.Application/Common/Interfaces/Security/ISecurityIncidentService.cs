using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Enums.Security;

namespace PayVerse.Application.Common.Interfaces.Security;

/// <summary>
/// Service for managing security incidents
/// </summary>
public interface ISecurityIncidentService
{
    /// <summary>
    /// Logs a new security incident
    /// </summary>
    /// <param name="type">Type of security incident</param>
    /// <param name="description">Description of the incident</param>
    /// <param name="userId">Optional ID of the affected user</param>
    /// <param name="ipAddress">IP address related to the incident</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created security incident</returns>
    Task<Guid> LogIncidentAsync(SecurityIncidentType type, string description, Guid? userId, string ipAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all pending security incidents
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of pending security incidents</returns>
    Task<IEnumerable<SecurityIncident>> GetPendingIncidentsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolves a security incident
    /// </summary>
    /// <param name="incidentId">ID of the incident to resolve</param>
    /// <param name="resolutionDetails">Details of how the incident was resolved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the incident was successfully resolved</returns>
    Task<bool> ResolveIncidentAsync(Guid incidentId, string resolutionDetails, CancellationToken cancellationToken = default);
}

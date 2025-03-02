using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Enums.Security;
using PayVerse.Domain.Repositories.Security;

namespace PayVerse.Infrastructure.Services.Security;

/// <summary>
/// Implementation of security incident service
/// </summary>
public class SecurityIncidentService(
    ISecurityIncidentRepository securityIncidentRepository,
    ISecurityIncidentResolutionRepository resolutionRepository,
    ICurrentUserService currentUserService,
    ILogger<SecurityIncidentService> logger) : ISecurityIncidentService
{
    public async Task<Guid> LogIncidentAsync(
        SecurityIncidentType type,
        string description,
        Guid? userId,
        string ipAddress,
        CancellationToken cancellationToken = default)
    {
        // This would use a factory method to create the SecurityIncident
        // For compatibility with the domain pattern shown in the provided code
        var incident = SecurityIncident.Create(
            Guid.NewGuid(),
            type, 
            description, 
            userId, 
            ipAddress);

        await securityIncidentRepository.SaveAsync(incident, cancellationToken);

        logger.LogWarning("Security incident logged: {Type} - {Description}", type, description);

        return incident.Id;
    }

    public async Task<IEnumerable<SecurityIncident>> GetPendingIncidentsAsync(CancellationToken cancellationToken = default)
    {
        // This would use a specification pattern or query to filter by status
        // Simplified implementation for example purposes
        var pendingIncidents = await securityIncidentRepository.GetAllAsync(cancellationToken);
        return pendingIncidents.Where(i => i.Status == SecurityIncidentStatus.Pending);
    }

    public async Task<bool> ResolveIncidentAsync(Guid incidentId, string resolutionDetails, CancellationToken cancellationToken = default)
    {
        var incident = await securityIncidentRepository.GetByIdAsync(incidentId, cancellationToken);
        if (incident is null)
        {
            logger.LogWarning("Attempt to resolve non-existent security incident with ID {IncidentId}", incidentId);
            return false;
        }

        // Create resolution (using domain logic)
        // Again, this would typically use a factory method
        var resolutionResult = incident.AddResolution(
            Guid.NewGuid(),
            resolutionDetails,
            currentUserService.UserId);

        // Update incident status
        // In a proper DDD implementation, this would be done through a domain method on the SecurityIncident aggregate

        await resolutionRepository.AddAsync(resolutionResult.Value, cancellationToken);

        logger.LogInformation("Security incident {IncidentId} resolved", incidentId);

        return true;
    }
}

using PayVerse.Domain.Entities.Security;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.Repositories.Security;

public interface ISecurityIncidentResolutionRepository
{
    Task AddAsync(SecurityIncidentResolution resolution, CancellationToken cancellationToken);
}
using PayVerse.Domain.Entities.Security;

namespace PayVerse.Domain.Repositories.Security;

public interface ISecurityIncidentRepository
{
    Task<IEnumerable<SecurityIncident>> GetAllAsync(CancellationToken cancellationToken);
    Task<SecurityIncident> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SecurityIncident>> GetPendingIncidentsAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(SecurityIncident incident, CancellationToken cancellationToken = default);
}

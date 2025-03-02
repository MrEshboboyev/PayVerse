
using PayVerse.Domain.Entities.Security;

namespace PayVerse.Domain.Repositories.Security;

public interface IBlockedIpRepository
{
    Task AddAsync(BlockedIp blockedIp, CancellationToken cancellationToken = default);
    Task<IEnumerable<BlockedIp>> GetAllAsync(CancellationToken cancellationToken);
    Task<BlockedIp> GetByIpAddressAsync(string ipAddress, CancellationToken cancellationToken = default);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}

using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.Chains.Payments.Services;

// Interfaces for external services (would be implemented in Infrastructure layer)
public interface IVirtualAccountChainService
{
    Task<VirtualAccount> GetVirtualAccountByUserId(Guid userId);
}

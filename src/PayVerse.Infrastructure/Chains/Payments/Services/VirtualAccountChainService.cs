using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Chains.Payments.Services;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Persistence;

namespace PayVerse.Infrastructure.Chains.Payments.Services;

/// <summary>
/// Virtual Account Chain Service implementations
/// </summary>
public class VirtualAccountChainService(ApplicationDbContext context) : IVirtualAccountChainService
{
    public async Task<VirtualAccount> GetVirtualAccountByUserId(Guid userId)
    {
        var virtualAccount = await context.Set<VirtualAccount>()
            .Include(va => va.Transactions) // Include related transactions
            .FirstOrDefaultAsync(va => va.UserId == userId) 
                ?? throw new Exception($"No virtual account found for user {userId}");
        
        return virtualAccount;
    }
}

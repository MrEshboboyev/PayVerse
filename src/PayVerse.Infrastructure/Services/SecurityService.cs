using PayVerse.Application.Common.Interfaces;
using System.Collections.Concurrent;

namespace PayVerse.Infrastructure.Services;

public class SecurityService : ISecurityService
{
    private static readonly ConcurrentDictionary<Guid, bool> FlaggedAccounts = new();

    public Task FlagAccountAsync(Guid userId)
    {
        FlaggedAccounts[userId] = true;
        return Task.CompletedTask;
    }

    public Task<bool> IsSuspiciousTransactionAsync(Guid userId, decimal amount)
    {
        const decimal SuspiciousAmountThreshold = 10000m;
        return Task.FromResult(amount >= SuspiciousAmountThreshold);
    }

    public Task<bool> IsUserAuthorizedAsync(Guid userId)
    {
        return Task.FromResult(!FlaggedAccounts.ContainsKey(userId));
    }
}

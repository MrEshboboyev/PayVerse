namespace PayVerse.Application.Common.Interfaces;

// Supporting interfaces for the Proxy implementation
public interface ISecurityService
{
    Task<bool> IsUserAuthorizedAsync(Guid userId);
    Task<bool> IsSuspiciousTransactionAsync(Guid userId, decimal amount);
    Task FlagAccountAsync(Guid userId);
}


using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Infrastructure.Proxies;

// ✅ Benefits:
// Ensures only the owner can execute wallet transactions.
// Adds an extra security layer without modifying the core transaction logic.

public interface IWalletTransaction
{
    void Execute(WalletTransaction transaction);
}

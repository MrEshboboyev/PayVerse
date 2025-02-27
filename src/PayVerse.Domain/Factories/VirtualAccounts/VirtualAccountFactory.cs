using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Domain.Factories.VirtualAccounts;

// ✅ Benefits:
// Ensures consistent creation logic(e.g., always starts with Balance = 0).
// Reduces constructor overload issues.
// Helps easily add features like setting an overdraft limit
// without modifying the VirtualAccount class.

public static class VirtualAccountFactory
{
    public static VirtualAccount Create(Guid userId, Currency currency)
    {
        var accountId = Guid.NewGuid();
        var accountNumber = AccountNumber.Generate(currency);
        var balance = Balance.Create(0).Value;

        return VirtualAccount.Create(
            accountId, 
            accountNumber, 
            currency, 
            balance, 
            userId);
    }
}

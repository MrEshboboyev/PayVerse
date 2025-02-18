using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Users;
using PayVerse.Domain.ValueObjects.Wallets;

namespace PayVerse.Domain.Factories.Users;

// ✅ Benefits:
// Ensures every new User always has a Wallet.
// Avoids having "orphaned" Users without wallets.
// Can easily extend to include other related objects(e.g., assigning a default role).

public interface IUserWalletFactory
{
    User CreateUser(
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName,
        Role role);

    Wallet CreateWallet(
        User user, 
        Currency currency);
}

public class UserWalletFactory : IUserWalletFactory
{
    public User CreateUser(
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName,
        Role role)
    {
        var user = User.Create(
            Guid.NewGuid(),
            email, 
            passwordHash,
            firstName,
            lastName, 
            role);

        return user;
    }

    public Wallet CreateWallet(
        User user,
        Currency currency)
    {
        return Wallet.Create(
            Guid.NewGuid(),
            WalletBalance.Create(0).Value,
            currency,
            user.Id); // Starts with 0 balance
    }
}

using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.VirtualAccounts.Directors;

/// <summary>
/// Director class for complex account creation scenarios
/// </summary>
public class AccountCreationDirector(
    IVirtualAccountRepository accountRepository,
    IWalletRepository walletRepository)
{

    /// <summary>
    /// Creates a complete financial package for a new business user
    /// </summary>
    public async Task<(Guid accountId, Guid walletId)> CreateBusinessFinancialPackage(Guid userId,
                                                                                      Currency preferredCurrency)
    {
        // Create a virtual account with overdraft
        var account = VirtualAccount.CreateBuilder(userId, preferredCurrency)
            .WithOverdraftLimit(5000)
            .Build();

        // Create a business wallet with spending limit
        var wallet = Wallet.CreateBuilder(userId, preferredCurrency)
            .WithSpendingLimit(10000)
            .WithLoyaltyPoints(100) // Welcome bonus
            .Build();

        await accountRepository.AddAsync(account);
        await walletRepository.AddAsync(wallet);

        return (account.Id, wallet.Id);
    }

    /// <summary>
    /// Creates a complete financial package for a new individual user
    /// </summary>
    public async Task<(Guid accountId, Guid walletId)> CreateIndividualFinancialPackage(Guid userId, Currency preferredCurrency)
    {
        // Create a virtual account
        var account = VirtualAccount.CreateBuilder(userId, preferredCurrency)
            .WithOverdraftLimit(1000)
            .Build();

        // Create a personal wallet
        var wallet = Wallet.CreateBuilder(userId, preferredCurrency)
            .WithSpendingLimit(2000)
            .WithLoyaltyPoints(50) // Welcome bonus
            .Build();

        await accountRepository.AddAsync(account);
        await walletRepository.AddAsync(wallet);

        return (account.Id, wallet.Id);
    }
}

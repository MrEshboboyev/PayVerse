using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Wallets;

namespace PayVerse.Domain.Builders.Wallets;

/// <summary>
/// Builder for creating Wallet entities
/// </summary>
/// <remarks>
/// Constructor with required parameters
/// </remarks>
public class WalletBuilder(Guid userId, Currency currency) : IBuilder<Wallet>
{
    #region Private Properties

    // Optional parameters with default values
    private WalletBalance _balance = WalletBalance.Create(0).Value;
    private decimal? _spendingLimit;
    private int _loyaltyPoints;
    private readonly List<InitialWalletTransactionDto> _initialTransactions = [];

    #endregion

    #region Building Blocks

    /// <summary>
    /// Sets the initial balance
    /// </summary>
    public WalletBuilder WithInitialBalance(decimal balance)
    {
        _balance = WalletBalance.Create(balance).Value;
        return this;
    }

    /// <summary>
    /// Sets a spending limit
    /// </summary>
    public WalletBuilder WithSpendingLimit(decimal limit)
    {
        if (limit < 0)
        {
            throw new ArgumentException("Spending limit must be a positive value");
        }

        _spendingLimit = limit;
        return this;
    }

    /// <summary>
    /// Sets initial loyalty points
    /// </summary>
    public WalletBuilder WithLoyaltyPoints(int points)
    {
        if (points < 0)
        {
            throw new ArgumentException("Loyalty points must be a positive value");
        }

        _loyaltyPoints = points;
        return this;
    }

    #endregion

    #region Transactions

    /// <summary>
    /// Adds an initial transaction to the wallet
    /// </summary>
    public WalletBuilder AddInitialTransaction(decimal amount,
                                               string description)
    {
        _initialTransactions.Add(new InitialWalletTransactionDto(amount, description));
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the Wallet instance
    /// </summary>
    public Wallet Build()
    {
        var wallet = Wallet.Create(
            Guid.NewGuid(),
            _balance,
            currency,
            userId);

        if (_spendingLimit.HasValue)
        {
            wallet.SetSpendingLimit(_spendingLimit.Value);
        }

        if (_loyaltyPoints > 0)
        {
            wallet.AddLoyaltyPoints(_loyaltyPoints);
        }

        // Add initial transactions
        foreach (var transaction in _initialTransactions)
        {
            wallet.AddTransaction(transaction.Amount,
                                  DateTime.UtcNow,
                                  transaction.Description);
        }

        return wallet;
    }

    #endregion
}

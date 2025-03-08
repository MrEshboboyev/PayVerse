using PayVerse.Domain.Builders.Wallets;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Wallets;
using PayVerse.Domain.Iterators;
using PayVerse.Domain.Iterators.Wallets;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Wallets;

namespace PayVerse.Domain.Entities.Wallets;

/// <summary>
/// Represents a wallet in the system with Prototype pattern implementation
/// </summary>
public sealed class Wallet : PrototypeAggregateRoot, IAuditableEntity, IIterable<WalletTransaction>
{
    #region Private Fields
    
    private readonly List<WalletTransaction> _transactions = [];
    
    #endregion

    #region Constructor
    
    private Wallet(
        Guid id,
        WalletBalance balance,
        Currency currency,
        Guid userId)
        : base(id)
    {
        Balance = balance;
        Currency = currency;
        UserId = userId;

        RaiseDomainEvent(new WalletCreatedDomainEvent(
            Guid.NewGuid(),
            id));
    }

    // Copy constructor for Prototype pattern
    private Wallet(Wallet source) : base(source.Id)
    {
        Balance = source.Balance;
        Currency = source.Currency;
        UserId = source.UserId;
        SpendingLimit = source.SpendingLimit;
        LoyaltyPoints = source.LoyaltyPoints;
        CreatedOnUtc = source.CreatedOnUtc;
        ModifiedOnUtc = source.ModifiedOnUtc;

        // Deep copy the transactions
        foreach (var transaction in source._transactions)
        {
            _transactions.Add(transaction.DeepCopy() as WalletTransaction);
        }
    }

    #endregion

    #region Properties

    public WalletBalance Balance { get; private set; }
    public Currency Currency { get; private set; }
    public Guid UserId { get; private set; }
    public decimal? SpendingLimit { get; private set; }
    public int LoyaltyPoints { get; private set; }
    public IReadOnlyCollection<WalletTransaction> Transactions => _transactions.AsReadOnly();
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    #endregion

    #region Factory Methods
    
    public static Wallet Create(
        Guid id,
        WalletBalance balance,
        Currency currency,
        Guid userId)
    {
        return new Wallet(id, balance, currency, userId);
    }

    #region Prototype related

    // Factory method to create from a prototype
    public static Wallet CreateFromPrototype(Wallet prototype)
    {
        return prototype.DeepCopy() as Wallet;
    }

    // Factory method to create a wallet with spending limit from a prototype
    public static Wallet CreateWithSpendingLimitFromPrototype(Wallet prototype,
                                                              decimal spendingLimit)
    {
        var newWallet = prototype.DeepCopy() as Wallet;
        newWallet.SetSpendingLimit(spendingLimit);
        return newWallet;
    }

    #endregion

    #endregion

    #region Own methods

    public Result ConvertCurrency(
        Currency newCurrency,
        decimal newBalance)
    {
        var oldCurrencyCode = Currency.Code;
        Balance = WalletBalance.Create(newBalance).Value;
        Currency = newCurrency;

        RaiseDomainEvent(new WalletCurrencyConvertedDomainEvent(
            Guid.NewGuid(),
            Id, 
            oldCurrencyCode, 
            newCurrency.Code));
        
        return Result.Success();
    }

    public Result SetSpendingLimit(decimal spendingLimit)
    {
        SpendingLimit = spendingLimit;

        RaiseDomainEvent(new SpendingLimitSetDomainEvent(
            Guid.NewGuid(), 
            Id,
            spendingLimit));
        
        return Result.Success();
    }

    // Additional methods to support the builder pattern
    public Result AddLoyaltyPoints(int points)
    {
        if (points < 0)
        {
            throw new ArgumentException("Loyalty points must be a positive value"); // write a domain error
        }

        LoyaltyPoints += points;

        return Result.Success();
    }

    public Result RedeemLoyaltyPoints(int points)
    {
        if (points > LoyaltyPoints)
        {
            return Result.Failure(
                DomainErrors.Wallet.InsufficientLoyaltyPoints(points, LoyaltyPoints));
        }

        LoyaltyPoints -= points;

        RaiseDomainEvent(new LoyaltyPointsRedeemedDomainEvent(
            Guid.NewGuid(), 
            Id,
            points));

        return Result.Success();
    }

    /// <summary>
    /// Adds funds to the wallet.
    /// </summary>
    /// <param name="amount">The amount to add.</param>
    /// <param name="description">A description of the transaction.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result AddFunds(
        Amount amount, 
        string description)
    {
        Balance = WalletBalance.Create(Balance.Value + amount.Value).Value;

        RaiseDomainEvent(new WalletFundsAddedDomainEvent(
            Guid.NewGuid(),
            Id, 
            amount.Value, 
            description));

        return Result.Success();
    }

    /// <summary>
    /// Deducts funds from the wallet.
    /// </summary>
    /// <param name="amount">The amount to deduct.</param>
    /// <param name="description">A description of the transaction.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result DeductFunds(
        Amount amount, 
        string description)
    {
        if (Balance.Value < amount.Value)
        {
            return Result.Failure(
                DomainErrors.Wallet.InsufficientFunds(Id));
        }

        Balance = WalletBalance.Create(Balance.Value - amount.Value).Value;

        RaiseDomainEvent(new WalletFundsDeductedDomainEvent(
            Guid.NewGuid(), 
            Id, 
            amount.Value, 
            description));

        return Result.Success();
    }

    #endregion

    #region Transaction related Methods

    public WalletTransaction GetTransactionById(Guid transactionId) => 
        _transactions.FirstOrDefault(t => t.Id == transactionId);

    public Result<WalletTransaction> AddTransaction(
        Amount amount,
        DateTime date,
        string description)
    {
        #region Create new transaction
        
        var transaction = new WalletTransaction(
            Guid.NewGuid(),
            Id,
            amount,
            date,
            description);
        
        #endregion
        
        #region Add transaction to wallet
        
        _transactions.Add(transaction);

        #endregion

        #region Update balance

        // Update balance
        Balance = WalletBalance.Create(Balance.Value + amount.Value).Value;

        #endregion

        #region Domain Events

        RaiseDomainEvent(new WalletTransactionAddedDomainEvent(
            Guid.NewGuid(),
            Id,
            transaction.Id));
        
        #endregion
        
        return Result.Success(transaction);
    }

    public Result RemoveTransaction(Guid transactionId)
    {
        #region Checking transaction exists
        
        var transaction = _transactions.FirstOrDefault(t => t.Id == transactionId);
        if (transaction is null)
        {
            return Result.Failure(
                DomainErrors.Wallet.TransactionNotFound(transactionId));
        }
        
        #endregion
        
        #region Remove transaction from wallet
        
        _transactions.Remove(transaction);
        
        #endregion
        
        #region Domain Events
        
        RaiseDomainEvent(new WalletTransactionRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            transaction.Id));
        
        #endregion
        
        return Result.Success();
    }

    #endregion

    #region Prototype overrides

    public override PrototypeAggregateRoot ShallowCopy()
    {
        return new Wallet(
            Id,
            Balance,
            Currency,
            UserId);
    }

    public override PrototypeAggregateRoot DeepCopy()
    {
        return new Wallet(this);
    }

    #endregion

    #region Builders

    // Factory method for the builder
    public static WalletBuilder CreateBuilder(Guid userId, Currency currency)
    {
        return new WalletBuilder(userId, currency);
    }

    #endregion

    #region Iterator Implementation

    /// <summary>
    /// Creates a default wallet transaction iterator
    /// </summary>
    /// <returns>Iterator over all wallet transactions</returns>
    public IIterator<WalletTransaction> CreateIterator()
    {
        return new WalletTransactionIterator(Transactions);
    }

    #endregion
}
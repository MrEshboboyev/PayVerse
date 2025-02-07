using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Wallets;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Wallets;

namespace PayVerse.Domain.Entities.Wallets;

/// <summary>
/// Represents a wallet in the system.
/// </summary>
public sealed class Wallet : AggregateRoot, IAuditableEntity
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
    
    #endregion

    #region Properties
    
    public WalletBalance Balance { get; private set; }
    public Currency Currency { get; private set; }
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<WalletTransaction> Transactions => _transactions.AsReadOnly();
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    #endregion

    #region Factory Method
    
    public static Wallet Create(
        Guid id,
        WalletBalance balance,
        Currency currency,
        Guid userId)
    {
        return new Wallet(id, balance, currency, userId);
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
}
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Domain.Entities.VirtualAccounts;

/// <summary>
/// Represents a virtual account in the system.
/// </summary>
public sealed class VirtualAccount : AggregateRoot, IAuditableEntity
{
    #region Private Fields
    
    private readonly List<Transaction> _transactions = [];
    
    #endregion

    #region Constructor
    
    private VirtualAccount(
        Guid id,
        AccountNumber accountNumber,
        Currency currency,
        Balance balance,
        Guid userId)
        : base(id)
    {
        AccountNumber = accountNumber;
        Currency = currency;
        Balance = balance;
        UserId = userId;

        RaiseDomainEvent(new VirtualAccountCreatedDomainEvent(
            Guid.NewGuid(),
            id,
            accountNumber.Value));
    }
    
    #endregion

    #region Properties
    
    public AccountNumber AccountNumber { get; private set; }
    public Currency Currency { get; private set; }
    public Balance Balance { get; private set; }
    public Guid UserId { get; private set; }
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    #endregion

    #region Factory Method
    
    public static VirtualAccount Create(
        Guid id,
        AccountNumber accountNumber,
        Currency currency,
        Balance balance,
        Guid userId)
    {
        return new VirtualAccount(id, accountNumber, currency, balance, userId);
    }
    
    #endregion

    #region Methods
    
    public Result<Transaction> AddTransaction(
        decimal amount,
        DateTime date,
        string description)
    {
        #region Create new Transaction
        
        var transaction = new Transaction(
            Guid.NewGuid(),
            amount,
            date,
            description);
        
        #endregion
        
        #region Add Transaction to this Account
        
        _transactions.Add(transaction);
        
        #endregion
        
        return Result.Success(transaction);
    } 
    
    #endregion
}
using PayVerse.Domain.Builders.VirtualAccounts;
using PayVerse.Domain.Enums.VirtualAccounts;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.VirtualAccounts;
using PayVerse.Domain.Iterators;
using PayVerse.Domain.Iterators.VirtualAccounts;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Prototypes;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.VirtualAccounts;
using PayVerse.Domain.Visitors;

namespace PayVerse.Domain.Entities.VirtualAccounts;

/// <summary>
/// Represents a virtual account in the system with (Prototype, Iterator, Visitor) pattern implementation
/// </summary>
public sealed class VirtualAccount : PrototypeAggregateRoot, IAuditableEntity, IIterable<Transaction>, IVisitable
{
    #region Private Fields
    
    private readonly List<Transaction> _transactions = [];

    #endregion

    #region Constructors

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
        Status = VirtualAccountStatus.Active;

        RaiseDomainEvent(new VirtualAccountCreatedDomainEvent(
            Guid.NewGuid(),
            id,
            accountNumber.Value));
    }

    // Copy constructor for Prototype pattern
    private VirtualAccount(VirtualAccount source) : base(source.Id)
    {
        AccountNumber = source.AccountNumber;
        Currency = source.Currency;
        Balance = source.Balance;
        UserId = source.UserId;
        Status = source.Status;
        OverdraftLimit = source.OverdraftLimit;
        CreatedOnUtc = source.CreatedOnUtc;
        ModifiedOnUtc = source.ModifiedOnUtc;

        // Deep copy the transactions
        foreach (var transaction in source._transactions)
        {
            _transactions.Add(transaction.DeepCopy() as Transaction);
        }
    }

    #endregion

    #region Properties

    public AccountNumber AccountNumber { get; private set; }
    public Currency Currency { get; private set; }
    public Balance Balance { get; private set; }
    public Guid UserId { get; private set; }
    public VirtualAccountStatus Status { get; private set; }
    public decimal OverdraftLimit { get; private set; }
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

    #region Prototype related

    // Factory method to create from a prototype
    public static VirtualAccount CreateFromPrototype(VirtualAccount prototype)
    {
        return prototype.DeepCopy() as VirtualAccount;
    }

    // Factory method to create a sub-account from a prototype
    public static VirtualAccount CreateSubAccountFromPrototype(VirtualAccount prototype,
                                                               AccountNumber newAccountNumber)
    {
        var newAccount = prototype.DeepCopy() as VirtualAccount;
        newAccount.AccountNumber = newAccountNumber;
        newAccount.Balance = Balance.Create(0).Value;
        return newAccount;
    }

    #endregion

    #endregion

    #region Own methods

    public Result Close()
    {
        if (Status is VirtualAccountStatus.Closed)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.AccountAlreadyClosed(Id));
        }

        Status = VirtualAccountStatus.Closed;
        
        RaiseDomainEvent(new VirtualAccountClosedDomainEvent(
            Guid.NewGuid(),
            Id));
        
        return Result.Success();
    }

    public Result Freeze()
    {
        if (Status is VirtualAccountStatus.Closed or VirtualAccountStatus.Frozen)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.AccountAlreadyClosedOrFrozen(Id));
        }

        Status = VirtualAccountStatus.Frozen;
        
        RaiseDomainEvent(new VirtualAccountFrozenDomainEvent(
            Guid.NewGuid(),
            Id));
        
        return Result.Success();
    }

    public Result Unfreeze()
    {
        if (Status is not VirtualAccountStatus.Frozen)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.AccountNotFrozen(Id));
        }

        Status = VirtualAccountStatus.Active;
        
        RaiseDomainEvent(new VirtualAccountUnfrozenDomainEvent(
            Guid.NewGuid(),
            Id));
        
        return Result.Success();
    }

    public Result TransferFunds(
        VirtualAccount toAccount,
        Amount amount)
    {
        if (Status is not VirtualAccountStatus.Active
            || toAccount.Status is not VirtualAccountStatus.Active)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.FromOrToAccountNotActive);
        }

        if (Balance.Value + OverdraftLimit < amount.Value)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.InsufficientFunds(Id));
        }

        #region Remove amount from this account
    
        var finalBalanceResult = Balance.Create(Balance.Value - amount.Value);
        if (finalBalanceResult.IsFailure)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.FinalBalanceFailure(amount.Value));
        }
        Balance = finalBalanceResult.Value;
    
        #endregion
    
        #region ToAccount balance updated

        var toAccountFinalBalanceResult = Balance.Create(toAccount.Balance.Value + amount.Value);
        if (toAccountFinalBalanceResult.IsFailure)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.FinalBalanceFailure(amount.Value));
        }
        toAccount.Balance = toAccountFinalBalanceResult.Value;

        #endregion

        #region Domain Events
        
        RaiseDomainEvent(new FundsTransferredDomainEvent(
            Guid.NewGuid(),
            Id,
            toAccount.Id,
            amount.Value));
        
        #endregion

        return Result.Success();
    }

    public Result SetOverdraftLimit(decimal overdraftLimit)
    {
        OverdraftLimit = overdraftLimit;
        
        RaiseDomainEvent(new OverdraftLimitSetDomainEvent(
            Guid.NewGuid(),
            Id,
            overdraftLimit));
        
        return Result.Success();
    }

    public Result SetStatus(VirtualAccountStatus status)
    {
        Status = status;

        //// Update state based on status (State pattern implementation)
        //_state = status switch
        //{
        //    VirtualAccountStatus.Active => new ActiveAccountState(this),
        //    VirtualAccountStatus.Frozen => new FrozenAccountState(this),
        //    VirtualAccountStatus.Closed => new ClosedAccountState(this),
        //    _ => throw new ArgumentOutOfRangeException(nameof(status))
        //};

        return Result.Success();
    }

    public Result UpdateBalance(Balance newBalance)
    {
        Balance = newBalance;
        return Result.Success();
    }

    /// <summary>
    /// Deposits an amount into the virtual account.
    /// </summary>
    /// <param name="amount">The amount to deposit.</param>
    /// <param name="description">A description of the deposit.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result Deposit(
        Amount amount,
        string description)
    {
        Balance = Balance.Create(Balance.Value + amount.Value).Value;

        RaiseDomainEvent(new VirtualAccountDepositedDomainEvent(
            Guid.NewGuid(),
            Id,
            amount.Value, 
            description));

        return Result.Success();
    }

    /// <summary>
    /// Withdraws an amount from the virtual account.
    /// </summary>
    /// <param name="amount">The amount to withdraw.</param>
    /// <param name="description">A description of the withdrawal.</param>
    /// <returns>Result indicating success or failure.</returns>
    public Result Withdraw(Amount amount, string description)
    {
        if (Balance.Value < amount.Value)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.InsufficientFunds(Id));
        }

        Balance = Balance.Create(Balance.Value - amount.Value).Value;
        
        RaiseDomainEvent(new VirtualAccountWithdrawnDomainEvent(
            Guid.NewGuid(),
            Id, 
            amount.Value, 
            description));

        return Result.Success();
    }


    #endregion

    #region Transaction related Methods

    public Transaction GetTransactionById(Guid transactionId) 
        => _transactions.FirstOrDefault(t => t.Id == transactionId);
    
    public Result<Transaction> AddTransaction(
        Amount amount,
        DateTime date,
        string description)
    {
        #region Create new Transaction
        
        var transaction = new Transaction(
            Guid.NewGuid(),
            Id,
            amount,
            date,
            description);
        
        #endregion
        
        #region Add Transaction to this Account
        
        _transactions.Add(transaction);

        #endregion

        #region Update Balance

        // Update balance
        Balance = Balance.Create(Balance.Value + amount.Value).Value;

        #endregion

        #region Domain Events

        RaiseDomainEvent(new TransactionAddedDomainEvent(
            Guid.NewGuid(),
            Id,
            transaction.Id));
        
        #endregion
        
        return Result.Success(transaction);
    }

    #endregion

    #region Prototype overrides

    public override PrototypeAggregateRoot ShallowCopy()
    {
        return new VirtualAccount(
            Id,
            AccountNumber,
            Currency,
            Balance,
            UserId);
    }

    public override PrototypeAggregateRoot DeepCopy()
    {
        return new VirtualAccount(this);
    }

    #endregion

    #region Builders

    // Factory method for the builder
    public static VirtualAccountBuilder CreateBuilder(Guid userId,
                                                      Currency currency)
    {
        return new VirtualAccountBuilder(userId, currency);
    }

    #endregion

    #region Iterator Implementation

    /// <summary>
    /// Creates a default transaction iterator
    /// </summary>
    /// <returns>Iterator over all transactions</returns>
    public IIterator<Transaction> CreateIterator()
    {
        return new TransactionIterator(Transactions);
    }

    /// <summary>
    /// Creates an iterator filtered by minimum transaction amount
    /// </summary>
    /// <param name="minimumAmount">Minimum amount to include in iteration</param>
    /// <returns>Filtered transaction iterator</returns>
    public IIterator<Transaction> CreateAmountFilteredIterator(decimal minimumAmount)
    {
        return new AmountFilteredTransactionIterator(Transactions, minimumAmount);
    }

    /// <summary>
    /// Creates an iterator that presents transactions ordered by date
    /// </summary>
    /// <returns>Date-ordered transaction iterator</returns>
    public IIterator<Transaction> CreateDateOrderedIterator()
    {
        return new DateOrderedTransactionIterator(Transactions);
    }

    #endregion

    #region Visitor Pattern Implementation

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }

    #endregion
}
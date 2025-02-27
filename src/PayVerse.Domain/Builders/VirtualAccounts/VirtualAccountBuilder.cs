using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Enums.VirtualAccounts;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Domain.Builders.VirtualAccounts;

/// <summary>
/// Builder for creating VirtualAccount entities
/// </summary>
/// <remarks>
/// Constructor with required parameters
/// </remarks>
public class VirtualAccountBuilder(Guid userId,
                                   Currency currency) : IBuilder<VirtualAccount>
{
    #region Private Properties

    // Optional parameters with default values
    private AccountNumber _accountNumber = AccountNumber.Generate(currency);
    private Balance _balance = Balance.Create(0).Value;
    private decimal _overdraftLimit;
    private VirtualAccountStatus _status = VirtualAccountStatus.Active;
    private readonly List<InitialTransactionDto> _initialTransactions = [];

    #endregion

    #region Building Blocks

    /// <summary>
    /// Sets a specific account number
    /// </summary>
    public VirtualAccountBuilder WithAccountNumber()
    {
        _accountNumber = AccountNumber.Generate(currency);
        return this;
    }

    /// <summary>
    /// Sets the initial balance
    /// </summary>
    public VirtualAccountBuilder WithInitialBalance(decimal balance)
    {
        _balance = Balance.Create(balance).Value;
        return this;
    }

    /// <summary>
    /// Sets an overdraft limit
    /// </summary>
    public VirtualAccountBuilder WithOverdraftLimit(decimal limit)
    {
        if (limit < 0)
        {
            throw new ArgumentException("Overdraft limit must be a positive value");
        }

        _overdraftLimit = limit;
        return this;
    }

    /// <summary>
    /// Sets the account status
    /// </summary>
    public VirtualAccountBuilder WithStatus(VirtualAccountStatus status)
    {
        _status = status;
        return this;
    }

    #endregion

    #region Transactions

    /// <summary>
    /// Adds an initial transaction to the account
    /// </summary>
    public VirtualAccountBuilder AddInitialTransaction(decimal amount,
                                                       string description)
    {
        _initialTransactions.Add(new InitialTransactionDto(amount, description));
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the VirtualAccount instance
    /// </summary>
    public VirtualAccount Build()
    {
        var account = VirtualAccount.Create(
            Guid.NewGuid(),
            _accountNumber,
            currency,
            _balance,
            userId);

        if (_overdraftLimit > 0)
        {
            account.SetOverdraftLimit(_overdraftLimit);
        }

        if (_status != VirtualAccountStatus.Active)
        {
            account.SetStatus(_status);
        }

        // Add initial transactions
        foreach (var transaction in _initialTransactions)
        {
            account.AddTransaction(transaction.Amount,
                                   DateTime.UtcNow,
                                   transaction.Description);
        }

        return account;
    }

    #endregion
}

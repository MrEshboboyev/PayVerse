using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.VirtualAccounts;

/// <summary>
/// Represents a virtual account number.
/// </summary>
public sealed class AccountNumber : ValueObject
{
    #region Constants
    
    public const int MaxLength = 16; // Assuming a 16-digit virtual account number
    
    #endregion
    
    #region Constructor
    
    private AccountNumber(string value)
    {
        Value = value;
    }
    
    #endregion

    #region Properties
    
    public string Value { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<AccountNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<AccountNumber>(
                DomainErrors.AccountNumber.Empty);
        }

        if (value.Length != MaxLength)
        {
            return Result.Failure<AccountNumber>(
                DomainErrors.AccountNumber.InvalidLength);
        }

        return Result.Success(new AccountNumber(value));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
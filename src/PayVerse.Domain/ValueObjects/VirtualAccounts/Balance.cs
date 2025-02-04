using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.VirtualAccounts;

/// <summary>
/// Represents the balance of a virtual account.
/// </summary>
public sealed class Balance : ValueObject
{
    #region Constructor
    
    private Balance(decimal value)
    {
        Value = value;
    }
    
    #endregion

    #region Properties
    
    public decimal Value { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<Balance> Create(decimal value)
    {
        if (value < 0)
        {
            return Result.Failure<Balance>(
                DomainErrors.Balance.Negative);
        }

        return Result.Success(new Balance(value));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
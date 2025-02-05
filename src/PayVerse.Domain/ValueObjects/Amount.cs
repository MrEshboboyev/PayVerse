using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects;

/// <summary>
/// Represents an amount of money.
/// </summary>
public sealed class Amount : ValueObject
{
    #region Constructor
    
    private Amount(decimal value)
    {
        Value = value;
    }
    
    #endregion

    #region Properties
    
    public decimal Value { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<Amount> Create(decimal value)
    {
        if (value < 0)
        {
            return Result.Failure<Amount>(
                DomainErrors.Amount.Negative);
        }

        return Result.Success(new Amount(value));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
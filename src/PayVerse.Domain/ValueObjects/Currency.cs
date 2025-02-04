using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects;

/// <summary>
/// Represents a currency type.
/// </summary>
public sealed class Currency : ValueObject
{
    #region Constants
    
    public const int CodeLength = 32;
    
    #endregion
    
    #region Constructor
    
    private Currency(string code)
    {
        Code = code;
    }
    
    #endregion

    #region Properties
    
    public string Code { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<Currency> Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != CodeLength)
        {
            return Result.Failure<Currency>(
                DomainErrors.Currency.Invalid);
        }

        return Result.Success(new Currency(code.ToUpper()));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Code;
    }
    
    #endregion
}
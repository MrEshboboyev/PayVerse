using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.Wallets;

/// <summary>
/// Represents the balance of a wallet.
/// </summary>
public sealed class WalletBalance : ValueObject
{
    #region Constructor
    
    private WalletBalance(decimal value)
    {
        Value = value;
    }
    
    #endregion

    #region Properties
    
    public decimal Value { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<WalletBalance> Create(decimal value)
    {
        if (value < 0)
        {
            return Result.Failure<WalletBalance>(
                DomainErrors.WalletBalance.Negative);
        }

        return Result.Success(new WalletBalance(value));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
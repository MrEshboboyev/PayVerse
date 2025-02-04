using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.Payments;

/// <summary>
/// Represents the amount of a payment.
/// </summary>
public sealed class PaymentAmount : ValueObject
{
    #region Constructor
    
    private PaymentAmount(decimal value)
    {
        Value = value;
    }
    
    #endregion

    #region Properties
    
    public decimal Value { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<PaymentAmount> Create(decimal value)
    {
        if (value <= 0)
        {
            return Result.Failure<PaymentAmount>(
                DomainErrors.PaymentAmount.Invalid);
        }

        return Result.Success(new PaymentAmount(value));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
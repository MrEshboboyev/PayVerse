using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.Invoices;

/// <summary>
/// Represents the date an invoice was issued.
/// </summary>
public sealed class InvoiceDate : ValueObject
{
    #region Constructor
    
    private InvoiceDate(DateTime value)
    {
        Value = value;
    }
    
    #endregion

    #region Properties
    
    public DateTime Value { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<InvoiceDate> Create(DateTime value)
    {
        if (value > DateTime.UtcNow)
        {
            return Result.Failure<InvoiceDate>(
                DomainErrors.InvoiceDate.FutureDate);
        }

        return Result.Success(new InvoiceDate(value));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
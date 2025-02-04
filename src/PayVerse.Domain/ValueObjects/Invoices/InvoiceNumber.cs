using PayVerse.Domain.Errors;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.ValueObjects.Invoices;

/// <summary>
/// Represents an invoice number.
/// </summary>
public sealed class InvoiceNumber : ValueObject
{
    #region Constants
    
    public const int MaxLength = 20;
    
    #endregion
    
    #region Constructor
    
    private InvoiceNumber(string value)
    {
        Value = value;
    }
    
    #endregion

    #region Properties
    
    public string Value { get; }
    
    #endregion

    #region Factory Method
    
    public static Result<InvoiceNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<InvoiceNumber>(
                DomainErrors.InvoiceNumber.Empty);
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<InvoiceNumber>(
                DomainErrors.InvoiceNumber.TooLong);
        }

        return Result.Success(new InvoiceNumber(value));
    }
    
    #endregion

    #region Overrides
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
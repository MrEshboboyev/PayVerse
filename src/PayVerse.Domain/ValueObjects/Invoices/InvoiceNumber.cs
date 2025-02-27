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

    #region Methods

    /// <summary>
    /// Generates a unique invoice number
    /// </summary>
    public static InvoiceNumber Generate()
    {
        var invoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-" +
            $"{Guid.NewGuid().ToString()[..8].ToUpper()}";
        return new InvoiceNumber(invoiceNumber);
    }

    #endregion

    #region Overrides

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
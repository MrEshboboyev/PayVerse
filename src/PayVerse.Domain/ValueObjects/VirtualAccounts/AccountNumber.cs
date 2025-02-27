using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.ValueObjects.VirtualAccounts;

/// <summary>
/// Represents a virtual account number.
/// </summary>
public sealed class AccountNumber : ValueObject
{
    #region Constants
    
    public const int Length = 16; // Assuming a 16-digit virtual account number
    
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

    #region Methods

    /// <summary>
    /// Generates a unique account number
    /// </summary>
    public static AccountNumber Generate(Currency currency)
    {
        var accountNumber = $"{currency.Code}" +
                            $"{DateTime.UtcNow:yyyyMMdd}" +
                            $"{new Random().Next(10000, 99999)}";

        return new AccountNumber(accountNumber);
    }

    #endregion

    #region Overrides

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}
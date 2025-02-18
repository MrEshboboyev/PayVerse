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

    public static AccountNumber Generate()
    {
        var random = new Random();
        var buffer = new char[Length];

        for (int i = 0; i < Length; i++)
        {
            buffer[i] = (char)('0' + random.Next(10)); // Generates a random digit between 0 and 9.
        }

        var accountNumber = new string(buffer);
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
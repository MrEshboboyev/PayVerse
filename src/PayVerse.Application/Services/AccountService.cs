using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Flyweights;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Services;

public class AccountService(CurrencyFlyweight currencyFlyweight)
{
    private readonly CurrencyFlyweight _currencyFlyweight = currencyFlyweight;

    #region Flyweight related

    public Result<VirtualAccount> CreateAccount(Guid userId, string currencyCode)
    {
        // Use Flyweight to get or create Currency
        var currencyResult = _currencyFlyweight.GetCurrency(currencyCode);

        if (currencyResult.IsFailure)
        {
            return Result.Failure<VirtualAccount>(currencyResult.Error);
        }

        // Proceed with account creation using the validated currency
        return Result.Success(VirtualAccount.Create(
            Guid.NewGuid(),
            null, // AccountNumber 
            currencyResult.Value,
            null, // Balance 
            userId
        ));
    }

    #endregion
}

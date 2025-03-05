using PayVerse.Application.Memento;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Flyweights;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Application.Services;

public class AccountService(CurrencyFlyweight currencyFlyweight)
{
    private readonly AccountCaretaker _caretaker = new();
    private readonly CurrencyFlyweight _currencyFlyweight = currencyFlyweight;

    public Result UpdateAccountBalance(VirtualAccount account, Balance newBalance)
    {
        // Save current state before any change
        var memento = account.SaveState();
        _caretaker.SaveState(memento);

        // Apply changes
        return account.UpdateBalance(newBalance);
    }

    public Result UndoLastBalanceChange(VirtualAccount account)
    {
        if (!_caretaker.CanUndo)
            return Result.Failure(DomainErrors.VirtualAccount.NoPreviousState);

        var memento = _caretaker.Undo();
        return account.RestoreState(memento);
    }

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

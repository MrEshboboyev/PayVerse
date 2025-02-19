using PayVerse.Application.Memento;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Application.Services;

public class AccountService
{
    private readonly AccountCaretaker _caretaker = new();

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
}

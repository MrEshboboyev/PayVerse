using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Shared;
using Microsoft.Extensions.Logging;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.Errors;

namespace PayVerse.Infrastructure.Handlers;

public class BalanceCheckHandler(ILogger<BalanceCheckHandler> logger) : BaseHandler
{
    private readonly ILogger<BalanceCheckHandler> _logger = logger;

    public override Result Handle(VirtualAccount account, Amount amount)
    {
        if (account.Balance.Value < amount.Value)
        {
            _logger.LogWarning("[Balance Check] Insufficient funds for account {AccountId}. Balance: {Balance}, Attempted Debit: {Amount}", account.Id, account.Balance.Value, amount.Value);
            return Result.Failure(
                DomainErrors.BalanceCheck.InsufficientFunds(account.Id, account.Balance.Value, amount.Value));
        }

        return base.Handle(account, amount);
    }
}

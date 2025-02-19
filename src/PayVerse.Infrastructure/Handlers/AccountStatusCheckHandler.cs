using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Shared;
using Microsoft.Extensions.Logging;
using PayVerse.Domain.Enums.VirtualAccounts;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.Errors;

namespace PayVerse.Infrastructure.Handlers;

public class AccountStatusCheckHandler(ILogger<AccountStatusCheckHandler> logger) : BaseHandler
{
    private readonly ILogger<AccountStatusCheckHandler> _logger = logger;

    public override Result Handle(VirtualAccount account, Amount amount)
    {
        if (account.Status != VirtualAccountStatus.Active)
        {
            _logger.LogWarning("[Account Status Check] Attempt to transact on inactive account {AccountId}", account.Id);
            return Result.Failure(
                DomainErrors.AccountStatusCheck.AccountInactive(account.Id));
        }

        return base.Handle(account, amount);
    }
}
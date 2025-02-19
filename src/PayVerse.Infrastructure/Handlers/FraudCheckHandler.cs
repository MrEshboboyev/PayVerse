using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Shared;
using Microsoft.Extensions.Logging;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.Errors;

namespace PayVerse.Infrastructure.Handlers;

public class FraudCheckHandler(ILogger<FraudCheckHandler> logger) : BaseHandler
{
    private readonly ILogger<FraudCheckHandler> _logger = logger;

    public override Result Handle(VirtualAccount account, Amount amount)
    {
        if (amount.Value > 10_000) // Example fraud rule
        {
            _logger.LogWarning("[Fraud Check] High-value transaction flagged for account {AccountId}. Amount: {Amount}", account.Id, amount.Value);
            return Result.Failure(
                DomainErrors.FraudCheck.TransactionFlagged(account.Id, amount.Value));
        }

        return base.Handle(account, amount);
    }
}

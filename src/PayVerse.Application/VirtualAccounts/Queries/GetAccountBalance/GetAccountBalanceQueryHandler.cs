using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetAccountBalance;

internal sealed class GetAccountBalanceQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetAccountBalanceQuery, decimal>
{
    public async Task<Result<decimal>> Handle(
        GetAccountBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;
        
        #region Get Virtual Account

        var virtualAccount = await virtualAccountRepository.GetByIdAsync(
            accountId,
            cancellationToken);
        if (virtualAccount is null)
        {
            return Result.Failure<decimal>(
                DomainErrors.VirtualAccount.NotFound(accountId));
        }

        #endregion

        return Result.Success(virtualAccount.Balance.Value);
    }
}

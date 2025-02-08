using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTotalDepositsAndWithdrawals;

internal sealed class GetTotalDepositsAndWithdrawalsQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetTotalDepositsAndWithdrawalsQuery, TotalDepositsAndWithdrawalsResponse>
{
    public async Task<Result<TotalDepositsAndWithdrawalsResponse>> Handle(
        GetTotalDepositsAndWithdrawalsQuery request,
        CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;

        var account = await virtualAccountRepository.GetByIdWithTransactionsAsync(
            accountId,
            cancellationToken);

        if (account is null)
        {
            return Result.Failure<TotalDepositsAndWithdrawalsResponse>(
                DomainErrors.VirtualAccount.NotFound(accountId));
        }

        var response = TotalDepositsAndWithdrawalsResponseFactory.Create(account);

        return Result.Success(response);
    }
}
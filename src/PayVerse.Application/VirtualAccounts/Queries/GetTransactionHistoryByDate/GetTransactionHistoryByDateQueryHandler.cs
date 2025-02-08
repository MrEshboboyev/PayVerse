using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTransactionHistoryByDate;

internal sealed class GetTransactionHistoryByDateQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) 
    : IQueryHandler<GetTransactionHistoryByDateQuery, TransactionListResponse>
{
    public async Task<Result<TransactionListResponse>> Handle(
        GetTransactionHistoryByDateQuery request,
        CancellationToken cancellationToken)
    {
        var (accountId, startDate, endDate) = request;

        var transactions = await virtualAccountRepository.GetTransactionsByDateAsync(
            accountId,
            startDate,
            endDate,
            cancellationToken);

        var response = new TransactionListResponse(
            transactions
                .Select(TransactionResponseFactory.Create)
                .ToList()
                .AsReadOnly());

        return Result.Success(response);
    }
}

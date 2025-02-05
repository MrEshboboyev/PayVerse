using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTransactions;

internal sealed class GetTransactionsQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetTransactionsQuery, TransactionListResponse>
{
    public async Task<Result<TransactionListResponse>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var virtualAccountId = request.VirtualAccountId;
        
        #region Get Virtual Account

        var virtualAccount = await virtualAccountRepository.GetByIdAsync(
            virtualAccountId,
            cancellationToken);
        if (virtualAccount is null)
        {
            return Result.Failure<TransactionListResponse>(
                DomainErrors.VirtualAccount.NotFound(virtualAccountId));
        }

        #endregion
        
        #region Prepare response
        
        var response = new TransactionListResponse(
            virtualAccount.Transactions
                .Select(TransactionResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        #endregion

        return Result.Success(response);
    }
}
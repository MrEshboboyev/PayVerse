using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountWithTransactionsById;

internal sealed class GetVirtualAccountWithTransactionsByIdQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetVirtualAccountWithTransactionsByIdQuery, VirtualAccountWithTransactionsResponse>
{
    public async Task<Result<VirtualAccountWithTransactionsResponse>> Handle(
        GetVirtualAccountWithTransactionsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var virtualAccountId = request.VirtualAccountId;
        
        // Get VirtualAccount
        var virtualAccountWithTransactions = await virtualAccountRepository.GetByIdWithTransactionsAsync(
            virtualAccountId,
            cancellationToken);
        if (virtualAccountWithTransactions is null)
        {
            return Result.Failure<VirtualAccountWithTransactionsResponse>(
                DomainErrors.VirtualAccount.NotFound(virtualAccountId));
        }
        
        // Prepare response
        var response = new VirtualAccountWithTransactionsResponse
        (
            VirtualAccountResponseFactory.Create(virtualAccountWithTransactions),
            virtualAccountWithTransactions.Transactions.Select(TransactionResponseFactory.Create)
                .ToList().AsReadOnly()
        );

        return Result.Success(response);
    }
}

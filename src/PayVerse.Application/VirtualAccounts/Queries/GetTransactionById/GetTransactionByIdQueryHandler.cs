using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetTransactionById;

internal sealed class GetTransactionByIdQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetTransactionByIdQuery, TransactionResponse>
{
    public async Task<Result<TransactionResponse>> Handle(
        GetTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var (virtualAccountId, transactionId) = request;
        
        var virtualAccount = await virtualAccountRepository.GetByIdWithTransactionsAsync(
            virtualAccountId, 
            cancellationToken);
        if (virtualAccount is null)
        {
            return Result.Failure<TransactionResponse>(
                DomainErrors.VirtualAccount.NotFound(virtualAccountId));
        }

        var transaction = virtualAccount.GetTransactionById(transactionId);
        if (transaction is null)
        {
            return Result.Failure<TransactionResponse>(
                DomainErrors.VirtualAccount.ItemNotFound(transactionId));
        }

        var response = TransactionResponseFactory.Create(transaction);

        return Result.Success(response);
    }
}

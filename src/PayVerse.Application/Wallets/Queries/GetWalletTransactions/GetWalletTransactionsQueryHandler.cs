using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Factories;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetWalletTransactions;

internal sealed class GetWalletTransactionsQueryHandler(
    IWalletRepository walletRepository) : IQueryHandler<GetWalletTransactionsQuery, WalletTransactionListResponse>
{
    public async Task<Result<WalletTransactionListResponse>> Handle(
        GetWalletTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var walletId = request.WalletId;
        
        #region Get this wallet
        
        var wallet = await walletRepository.GetByIdAsync(walletId, cancellationToken);
        if (wallet is null)
        {
            return Result.Failure<WalletTransactionListResponse>(
                DomainErrors.Wallet.NotFound(walletId));
        }
        
        #endregion
        
        #region Prepare response
        
        var response = new WalletTransactionListResponse(
            wallet.Transactions
                .Select(WalletTransactionResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        #endregion
        
        return Result.Success(response);
    }
}
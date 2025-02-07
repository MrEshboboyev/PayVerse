using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Factories;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetWalletWithTransactionsById;

internal sealed class GetWalletWithTransactionsByIdQueryHandler(
    IWalletRepository walletRepository) : IQueryHandler<GetWalletWithTransactionsByIdQuery, WalletWithTransactionsResponse>
{
    public async Task<Result<WalletWithTransactionsResponse>> Handle(
        GetWalletWithTransactionsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var walletId = request.WalletId;
        
        #region Get this wallet
        
        var wallet = await walletRepository.GetByIdWithTransactionsAsync(
            walletId,
            cancellationToken);
        if (wallet is null)
        {
            return Result.Failure<WalletWithTransactionsResponse>(
                DomainErrors.Wallet.NotFound(walletId));
        }
        
        #endregion
        
        #region Prepare response
        
        var response = new WalletWithTransactionsResponse(
            WalletResponseFactory.Create(wallet),
            wallet.Transactions
                .Select(WalletTransactionResponseFactory.Create)
                .ToList().AsReadOnly());
        
        #endregion
        
        return Result.Success(response);
    }
}
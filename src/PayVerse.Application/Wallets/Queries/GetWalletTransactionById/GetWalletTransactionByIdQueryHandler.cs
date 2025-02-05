using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Factories;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetWalletTransactionById;

internal sealed class GetWalletTransactionByIdQueryHandler(
    IWalletRepository walletRepository) : IQueryHandler<GetWalletTransactionByIdQuery, WalletTransactionResponse>
{
    public async Task<Result<WalletTransactionResponse>> Handle(
        GetWalletTransactionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var (walletId, transactionId) = request;

        #region Get this Wallet 
        
        var wallet = await walletRepository.GetByIdWithTransactionsAsync(
            walletId,
            cancellationToken);
        if (wallet is null)
        {
            return Result.Failure<WalletTransactionResponse>(
                DomainErrors.Wallet.NotFound(walletId));
        }
        
        #endregion
        
        #region Get this transaction from wallet
        
        var walletTransaction = wallet.GetTransactionById(transactionId);
        if (walletTransaction is null)
        {
            return Result.Failure<WalletTransactionResponse>(
                DomainErrors.Wallet.TransactionNotFound(transactionId));
        }
        
        #endregion

        var response = WalletTransactionResponseFactory.Create(walletTransaction);

        return Result.Success(response);
    }
}

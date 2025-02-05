using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Factories;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetWalletById;

internal sealed class GetWalletByIdQueryHandler(
    IWalletRepository walletRepository) : IQueryHandler<GetWalletByIdQuery, WalletResponse>
{
    public async Task<Result<WalletResponse>> Handle(
        GetWalletByIdQuery request,
        CancellationToken cancellationToken)
    {
        var walletId = request.WalletId;
        
        #region Get this wallet
        
        var wallet = await walletRepository.GetByIdAsync(walletId, cancellationToken);
        if (wallet is null)
        {
            return Result.Failure<WalletResponse>(
                DomainErrors.Wallet.NotFound(walletId));
        }
        
        #endregion
        
        #region Prepare response
        
        var response = WalletResponseFactory.Create(wallet);
        
        #endregion
        
        return Result.Success(response);
    }
}
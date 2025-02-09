using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetLoyaltyPointsBalance;

internal sealed class GetLoyaltyPointsBalanceQueryHandler(
    IWalletRepository walletRepository) : IQueryHandler<GetLoyaltyPointsBalanceQuery, int>
{
    public async Task<Result<int>> Handle(
        GetLoyaltyPointsBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var walletId = request.WalletId;
        
        #region Get this wallet

        var wallet = await walletRepository.GetByIdAsync(walletId, cancellationToken);
        if (wallet is null)
        {
            return Result.Failure<int>(
                DomainErrors.Wallet.NotFound(walletId));
        }

        #endregion

        return Result.Success(wallet.LoyaltyPoints);
    }
}

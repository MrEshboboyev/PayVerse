using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Commands.RedeemLoyaltyPoints;

internal sealed class RedeemLoyaltyPointsCommandHandler(
    IWalletRepository walletRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RedeemLoyaltyPointsCommand>
{
    public async Task<Result> Handle(
        RedeemLoyaltyPointsCommand request,
        CancellationToken cancellationToken)
    {
        var (walletId, points) = request;
        
        #region Get this Wallet
        
        var wallet = await walletRepository.GetByIdAsync(
            walletId,
            cancellationToken);
        if (wallet is null)
        {
            return Result.Failure(
                DomainErrors.Wallet.NotFound(walletId));
        }
        
        #endregion
        
        #region Redeem Loyal Points

        var redeemResult = wallet.RedeemLoyaltyPoints(points);
        if (redeemResult.IsFailure)
        {
            return Result.Failure(redeemResult.Error);
        }
        
        #endregion

        await walletRepository.UpdateAsync(wallet, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
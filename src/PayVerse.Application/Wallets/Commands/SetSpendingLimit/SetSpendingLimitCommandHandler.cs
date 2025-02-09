using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Commands.SetSpendingLimit;

internal sealed class SetSpendingLimitCommandHandler(
    IWalletRepository walletRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SetSpendingLimitCommand>
{
    public async Task<Result> Handle(
        SetSpendingLimitCommand request,
        CancellationToken cancellationToken)
    {
        var (walletId, spendingLimit) = request;
        
        #region Get Wallet
        
        var wallet = await walletRepository.GetByIdAsync(
            walletId,
            cancellationToken);
        if (wallet is null)
        {
            return Result.Failure(
                DomainErrors.Wallet.NotFound(walletId));
        }
        
        #endregion
        
        #region Set Spending Limit

        var setSpendingLimitResult = wallet.SetSpendingLimit(spendingLimit);
        if (setSpendingLimitResult.IsFailure)
        {
            return Result.Failure(setSpendingLimitResult.Error);
        }
        
        #endregion

        await walletRepository.UpdateAsync(wallet, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
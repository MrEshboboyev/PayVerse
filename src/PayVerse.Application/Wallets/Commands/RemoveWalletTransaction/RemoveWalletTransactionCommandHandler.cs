using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Commands.RemoveWalletTransaction;

internal sealed class RemoveWalletTransactionCommandHandler(
    IWalletRepository walletRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveWalletTransactionCommand>
{
    public async Task<Result> Handle(
        RemoveWalletTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var (walletId, transactionId) = request;

        #region Get Wallet

        var wallet = await walletRepository.GetByIdAsync(walletId, cancellationToken);
        if (wallet is null)
        {
            return Result.Failure(
                DomainErrors.Wallet.NotFound(walletId));
        }

        #endregion

        #region Remove Transaction

        var removalResult = wallet.RemoveTransaction(transactionId);
        if (removalResult.IsFailure)
        {
            return Result.Failure(removalResult.Error);
        }

        #endregion

        #region Save Changes

        await walletRepository.UpdateAsync(wallet, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}

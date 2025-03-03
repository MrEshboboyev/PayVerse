using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Shared;
using PayVerse.Domain.Errors;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Wallets.Commands.DeductFundsFromWallet;

internal sealed class DeductFundsFromWalletCommandHandler(
    IWalletRepository walletRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeductFundsFromWalletCommand>
{
    public async Task<Result> Handle(
        DeductFundsFromWalletCommand request,
        CancellationToken cancellationToken)
    {
        var (walletId, amountValue, description) = request;

        var wallet = await walletRepository.GetByIdAsync(walletId, cancellationToken);

        if (wallet is null)
        {
            return Result.Failure(
                DomainErrors.Wallet.NotFound(walletId));
        }

        var amountResult = Amount.Create(amountValue);
        if (amountResult.IsFailure)
        {
            return Result.Failure(amountResult.Error);
        }

        var result = wallet.DeductFunds(amountResult.Value, description);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await walletRepository.UpdateAsync(wallet, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

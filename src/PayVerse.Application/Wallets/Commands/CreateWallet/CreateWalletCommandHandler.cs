using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Wallets;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Wallets;

namespace PayVerse.Application.Wallets.Commands.CreateWallet;

internal sealed class CreateWalletCommandHandler(
    IWalletRepository walletRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateWalletCommand>
{
    public async Task<Result> Handle(
        CreateWalletCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, initialBalance, currencyCode) = request;

        #region Prepare value objects

        var balanceResult = WalletBalance.Create(initialBalance);
        if (balanceResult.IsFailure)
        {
            return Result.Failure(balanceResult.Error);
        }

        var currencyResult = Currency.Create(currencyCode);
        if (currencyResult.IsFailure)
        {
            return Result.Failure(currencyResult.Error);
        }

        #endregion

        #region Create Wallet

        var wallet = Wallet.Create(
            Guid.NewGuid(),
            balanceResult.Value,
            currencyResult.Value,
            userId);

        #endregion

        #region Add Wallet to Repository

        await walletRepository.AddAsync(wallet, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}

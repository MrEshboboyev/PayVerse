using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Wallets.Commands.AddWalletTransaction;

internal sealed class AddWalletTransactionCommandHandler(
    IWalletRepository walletRepository,
    IWalletTransactionRepository walletTransactionRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddWalletTransactionCommand>
{
    public async Task<Result> Handle(
        AddWalletTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var (walletId, amount, date, description) = request;
        
        #region Prepare value objects
        
        var amountResult = Amount.Create(amount);
        if (amountResult.IsFailure)
        {
            return Result.Failure(amountResult.Error);
        }
        
        #endregion
        
        #region Get Wallet

        var wallet = await walletRepository.GetByIdWithTransactionsAsync(
            walletId,
            cancellationToken);
        if (wallet is null)
        {
            return Result.Failure(
                DomainErrors.Wallet.NotFound(walletId));
        }

        #endregion

        #region Add Transaction

        var transactionResult = wallet.AddTransaction(amountResult.Value, date, description);
        if (transactionResult.IsFailure)
        {
            return Result.Failure(transactionResult.Error);
        }

        #endregion

        #region Save Changes

        await walletTransactionRepository.AddAsync(transactionResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}

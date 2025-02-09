using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Converters;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Wallets.Commands.ConvertWalletCurrency;

internal sealed class ConvertWalletCurrencyCommandHandler(
    IWalletRepository walletRepository,
    ICurrencyConverter currencyConverter,
    IUnitOfWork unitOfWork) : ICommandHandler<ConvertWalletCurrencyCommand>
{
    public async Task<Result> Handle(
        ConvertWalletCurrencyCommand request,
        CancellationToken cancellationToken)
    {
        var (walletId, newCurrencyCode) = request;
        
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
        
        #region Prepare value objects

        var newCurrencyResult = Currency.Create(newCurrencyCode);
        if (newCurrencyResult.IsFailure)
        {
            return Result.Failure(newCurrencyResult.Error);
        }
        
        #endregion
        
        #region Convert Currency

        var conversionResult = await currencyConverter.ConvertAsync(
            wallet.Balance.Value,
            wallet.Currency.Code,
            newCurrencyResult.Value.Code,
            cancellationToken);
        if (conversionResult.IsFailure)
        {
            return Result.Failure(conversionResult.Error);
        }
        
        #endregion
        
        #region Update Wallet

        var convertCurrencyResult = wallet.ConvertCurrency(
            newCurrencyResult.Value,
            conversionResult.Value);
        if (convertCurrencyResult.IsFailure)
        {
            return Result.Failure(convertCurrencyResult.Error);
        }
        
        #endregion

        await walletRepository.UpdateAsync(wallet, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
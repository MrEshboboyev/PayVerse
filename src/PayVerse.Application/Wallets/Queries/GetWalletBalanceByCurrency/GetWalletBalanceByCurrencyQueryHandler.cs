using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Converters;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetWalletBalanceByCurrency;

internal sealed class GetWalletBalanceByCurrencyQueryHandler(
    IWalletRepository walletRepository,
    ICurrencyConverter currencyConverter) : IQueryHandler<GetWalletBalanceByCurrencyQuery, decimal>
{
    public async Task<Result<decimal>> Handle(
        GetWalletBalanceByCurrencyQuery request,
        CancellationToken cancellationToken)
    {
        var (walletId, currencyCode) = request;
        
        #region Get this wallet

        var wallet = await walletRepository.GetByIdAsync(walletId, cancellationToken);
        if (wallet is null)
        {
            return Result.Failure<decimal>(
                DomainErrors.Wallet.NotFound(walletId));
        }

        #endregion
        
        #region Convert balance to requested currency

        var conversionResult = await currencyConverter.ConvertAsync(
            wallet.Balance.Value,
            wallet.Currency.Code,
            currencyCode,
            cancellationToken);
        if (conversionResult.IsFailure)
        {
            return Result.Failure<decimal>(
                conversionResult.Error);
        }

        #endregion

        return Result.Success(conversionResult.Value);
    }
}

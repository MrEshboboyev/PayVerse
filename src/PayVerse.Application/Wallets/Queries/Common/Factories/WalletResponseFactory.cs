using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Application.Wallets.Queries.Common.Factories;

public static class WalletResponseFactory
{
    public static WalletResponse Create(Wallet wallet)
    {
        return new WalletResponse(
            wallet.Id,
            wallet.Balance.Value,
            wallet.Currency.Code,
            wallet.UserId,
            wallet.CreatedOnUtc,
            wallet.ModifiedOnUtc);
    }
}
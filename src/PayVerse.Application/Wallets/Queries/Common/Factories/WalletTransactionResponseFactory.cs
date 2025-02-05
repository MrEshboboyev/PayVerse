using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Entities.Wallets;

namespace PayVerse.Application.Wallets.Queries.Common.Factories;

public static class WalletTransactionResponseFactory
{
    public static WalletTransactionResponse Create(WalletTransaction transaction)
    {
        return new WalletTransactionResponse(
            transaction.Id,
            transaction.Amount.Value,
            transaction.Date,
            transaction.Description);
    }
}
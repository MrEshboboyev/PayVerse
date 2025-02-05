using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Queries.Common.Factories;

public static class TransactionResponseFactory
{
    public static TransactionResponse Create(Transaction transaction)
    {
        return new TransactionResponse(
            transaction.Id,
            transaction.Amount.Value,
            transaction.Description);
    }
}
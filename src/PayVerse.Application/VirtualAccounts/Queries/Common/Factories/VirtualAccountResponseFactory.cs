using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Queries.Common.Factories;

public static class VirtualAccountResponseFactory
{
    public static VirtualAccountResponse Create(VirtualAccount virtualAccount)
    {
        return new VirtualAccountResponse(
            virtualAccount.Id,
            virtualAccount.AccountNumber.Value,
            virtualAccount.Currency.Code,
            virtualAccount.Balance.Value,
            virtualAccount.UserId,
            virtualAccount.CreatedOnUtc,
            virtualAccount.ModifiedOnUtc);
    }
}
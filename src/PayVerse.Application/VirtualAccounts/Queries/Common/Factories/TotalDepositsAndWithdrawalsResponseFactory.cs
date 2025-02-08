using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Queries.Common.Factories;

public static class TotalDepositsAndWithdrawalsResponseFactory
{
    public static TotalDepositsAndWithdrawalsResponse Create(VirtualAccount account)
    {
        var deposits = account.Transactions.Where(t => t.Amount.Value > 0).Sum(t => t.Amount.Value);
        var withdrawals = account.Transactions.Where(t => t.Amount.Value < 0).Sum(t => t.Amount.Value);

        var response = new TotalDepositsAndWithdrawalsResponse(deposits, withdrawals);
        
        return response;
    }
}
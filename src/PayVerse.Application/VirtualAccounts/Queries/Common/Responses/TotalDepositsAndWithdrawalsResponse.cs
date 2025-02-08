namespace PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

public sealed record TotalDepositsAndWithdrawalsResponse(
    decimal Deposits,
    decimal Withdrawals);
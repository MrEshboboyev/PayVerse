namespace PayVerse.Application.VirtualAccounts.Queries.Common.Responses;

public sealed record VirtualAccountListResponse(IReadOnlyCollection<VirtualAccountResponse> Accounts);
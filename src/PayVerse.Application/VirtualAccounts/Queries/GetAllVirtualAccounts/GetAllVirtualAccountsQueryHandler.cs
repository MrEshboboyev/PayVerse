using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetAllVirtualAccounts;

internal sealed class GetAllVirtualAccountsQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetAllVirtualAccountsQuery, VirtualAccountListResponse>
{
    public async Task<Result<VirtualAccountListResponse>> Handle(
        GetAllVirtualAccountsQuery request,
        CancellationToken cancellationToken)
    {
        var virtualAccounts = await virtualAccountRepository.GetAllAsync(cancellationToken);
        
        var response = new VirtualAccountListResponse(
            virtualAccounts
                .Select(VirtualAccountResponseFactory.Create)
                .ToList());
        
        return Result.Success(response);
    }
}

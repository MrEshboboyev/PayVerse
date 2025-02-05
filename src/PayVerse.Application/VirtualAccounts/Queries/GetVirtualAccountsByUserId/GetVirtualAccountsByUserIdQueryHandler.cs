using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountsByUserId;

internal sealed class GetVirtualAccountsByUserIdQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetVirtualAccountsByUserIdQuery, VirtualAccountListResponse>
{
    public async Task<Result<VirtualAccountListResponse>> Handle(
        GetVirtualAccountsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        var virtualAccounts = await virtualAccountRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);
        
        var response = new VirtualAccountListResponse(
            virtualAccounts
                .Select(VirtualAccountResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}

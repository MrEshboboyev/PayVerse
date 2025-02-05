using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Factories;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetWalletsByUserId;

internal sealed class GetWalletsByUserIdQueryHandler(
    IWalletRepository walletRepository) : IQueryHandler<GetWalletsByUserIdQuery, WalletListResponse>
{
    public async Task<Result<WalletListResponse>> Handle(
        GetWalletsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        var wallets = await walletRepository.GetAllByUserIdAsync(
            userId,
            cancellationToken);
        
        var response = new WalletListResponse(
            wallets
                .Select(WalletResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}

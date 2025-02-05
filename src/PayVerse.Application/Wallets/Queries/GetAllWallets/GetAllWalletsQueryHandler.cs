using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Wallets.Queries.Common.Factories;
using PayVerse.Application.Wallets.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Wallets.Queries.GetAllWallets;

internal sealed class GetAllWalletsQueryHandler(
    IWalletRepository walletRepository) : IQueryHandler<GetAllWalletsQuery, WalletListResponse>
{
    public async Task<Result<WalletListResponse>> Handle(
        GetAllWalletsQuery request,
        CancellationToken cancellationToken)
    {
        var wallets = await walletRepository.GetAllAsync(cancellationToken);
        
        var response = new WalletListResponse(
            wallets
                .Select(WalletResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        return Result.Success(response);
    }
}

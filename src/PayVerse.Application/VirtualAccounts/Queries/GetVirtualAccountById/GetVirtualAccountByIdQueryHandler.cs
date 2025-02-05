using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.VirtualAccounts.Queries.Common.Factories;
using PayVerse.Application.VirtualAccounts.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Queries.GetVirtualAccountById;

internal sealed class GetVirtualAccountByIdQueryHandler(
    IVirtualAccountRepository virtualAccountRepository) : IQueryHandler<GetVirtualAccountByIdQuery, VirtualAccountResponse>
{
    public async Task<Result<VirtualAccountResponse>> Handle(
        GetVirtualAccountByIdQuery request,
        CancellationToken cancellationToken)
    {
        var virtualAccountId = request.VirtualAccountId;
        
        #region Get Virtual Account

        var virtualAccount = await virtualAccountRepository.GetByIdAsync(
            virtualAccountId, 
            cancellationToken);
        if (virtualAccount is null)
        {
            return Result.Failure<VirtualAccountResponse>(
                DomainErrors.VirtualAccount.NotFound(virtualAccountId));
        }

        #endregion
        
        #region Prepare response
        
        var response = VirtualAccountResponseFactory.Create(virtualAccount);
        
        #endregion

        return Result.Success(response);
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Commands.FreezeVirtualAccount;

internal sealed class FreezeVirtualAccountCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<FreezeVirtualAccountCommand>
{
    public async Task<Result> Handle(
        FreezeVirtualAccountCommand request,
        CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;
        
        #region Get this Account
        
        var account = await virtualAccountRepository.GetByIdAsync(
            accountId,
            cancellationToken);
        if (account is null)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.NotFound(request.AccountId));
        }
        
        #endregion
        
        #region Freeze VirtualAccount

        var freezeAccountResult = account.Freeze();
        if (freezeAccountResult.IsFailure)
        {
            return Result.Failure(freezeAccountResult.Error);
        }
        
        #endregion

        await virtualAccountRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

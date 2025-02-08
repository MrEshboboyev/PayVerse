using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Commands.UnfreezeVirtualAccount;

internal sealed class UnfreezeVirtualAccountCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UnfreezeVirtualAccountCommand>
{
    public async Task<Result> Handle(
        UnfreezeVirtualAccountCommand request,
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
        
        #region Unfreeze VirtualAccount

        var unfreezeAccountResult = account.Unfreeze();
        if (unfreezeAccountResult.IsFailure)
        {
            return Result.Failure(unfreezeAccountResult.Error);
        }
        
        #endregion

        await virtualAccountRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
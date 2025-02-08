using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Commands.CloseVirtualAccount;

internal sealed class CloseVirtualAccountCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CloseVirtualAccountCommand>
{
    public async Task<Result> Handle(
        CloseVirtualAccountCommand request,
        CancellationToken cancellationToken)
    {
        var accountId = request.AccountId;
        
        #region Get this account
        
        var account = await virtualAccountRepository.GetByIdAsync(
            accountId,
            cancellationToken);
        if (account is null)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.NotFound(accountId));
        }
        
        #endregion
        
        #region Close this account

        var closeAccountResult = account.Close();
        if (closeAccountResult.IsFailure)
        {
            return Result.Failure(closeAccountResult.Error);
        }
        
        #endregion

        await virtualAccountRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

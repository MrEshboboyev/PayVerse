using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.VirtualAccounts.Commands.SetOverdraftLimit;

internal sealed class SetOverdraftLimitCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SetOverdraftLimitCommand>
{
    public async Task<Result> Handle(
        SetOverdraftLimitCommand request,
        CancellationToken cancellationToken)
    {
        var (accountId, overdraftLimit) = request;
        
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
        
        #region Update overdraft limit

        var setOverdraftLimitResult = account.SetOverdraftLimit(overdraftLimit);
        if (setOverdraftLimitResult.IsFailure)
        {
            return Result.Failure(setOverdraftLimitResult.Error);
        }
        
        #endregion

        await virtualAccountRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

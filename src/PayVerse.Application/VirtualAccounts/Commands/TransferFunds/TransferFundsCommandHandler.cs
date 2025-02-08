using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Enums.VirtualAccounts;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.VirtualAccounts.Commands.TransferFunds;

internal sealed class TransferFundsCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<TransferFundsCommand>
{
    public async Task<Result> Handle(
        TransferFundsCommand request,
        CancellationToken cancellationToken)
    {
        var (fromAccountId, toAccountId, amount) = request;
        
        #region Get this Accounts
        
        var fromAccount = await virtualAccountRepository.GetByIdAsync(
            fromAccountId,
            cancellationToken);
        
        if (fromAccount is null)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.NotFound(fromAccountId));
        }
        
        var toAccount = await virtualAccountRepository.GetByIdAsync(
            toAccountId,
            cancellationToken);

        if (toAccount is null)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.NotFound(fromAccountId));
        }
        
        #endregion

        #region Checking Accounts are Active

        if (fromAccount.Status is not VirtualAccountStatus.Active
            || toAccount.Status is not VirtualAccountStatus.Active)
        {
            return Result.Failure(DomainErrors.VirtualAccount.FromOrToAccountNotActive);
        }

        #endregion
        
        #region Prepare value objects

        var amountResult = Amount.Create(amount);
        if (amountResult.IsFailure)
        {
            return Result.Failure(amountResult.Error);
        }
        
        #endregion

        #region Transfer Funds
        
        var transferFundsResult = fromAccount.TransferFunds(toAccount, amountResult.Value);
        if (transferFundsResult.IsFailure)
        {
            return Result.Failure(transferFundsResult.Error);
        }
        
        #endregion

        await virtualAccountRepository.UpdateAsync(fromAccount, cancellationToken);
        await virtualAccountRepository.UpdateAsync(toAccount, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
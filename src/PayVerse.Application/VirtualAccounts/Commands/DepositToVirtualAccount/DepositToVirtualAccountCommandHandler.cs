using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Shared;
using PayVerse.Domain.Errors;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.VirtualAccounts.Commands.DepositToVirtualAccount;

internal sealed class DepositToVirtualAccountCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DepositToVirtualAccountCommand>
{
    public async Task<Result> Handle(
        DepositToVirtualAccountCommand request,
        CancellationToken cancellationToken)
    {
        var (accountId, amountValue, description) = request;

        var account = await virtualAccountRepository.GetByIdAsync(accountId, cancellationToken);

        if (account is null)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.NotFound(accountId));
        }

        var amountResult = Amount.Create(amountValue);
        if (amountResult.IsFailure)
        {
            return Result.Failure(amountResult.Error);
        }

        var result = account.Deposit(amountResult.Value, description);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await virtualAccountRepository.UpdateAsync(account, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

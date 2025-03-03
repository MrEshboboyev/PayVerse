using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Shared;
using PayVerse.Domain.Errors;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.VirtualAccounts.Commands.TransferBetweenAccounts;

internal sealed class TransferBetweenAccountsCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<TransferBetweenAccountsCommand>
{
    public async Task<Result> Handle(
        TransferBetweenAccountsCommand request,
        CancellationToken cancellationToken)
    {
        var (fromAccountId, toAccountId, amountValue, description) = request;

        // Validate accounts are different
        if (fromAccountId == toAccountId)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.SameAccountTransfer(fromAccountId));
        }

        // Begin transaction to ensure atomicity
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var sourceAccount = await virtualAccountRepository.GetByIdAsync(
                fromAccountId, 
                cancellationToken);
            if (sourceAccount is null)
            {
                return Result.Failure(
                    DomainErrors.VirtualAccount.SourceAccountNotFound(fromAccountId));
            }

            var destinationAccount = await virtualAccountRepository.GetByIdAsync(
                toAccountId, 
                cancellationToken);
            if (destinationAccount is null)
            {
                return Result.Failure(
                    DomainErrors.VirtualAccount.DestinationAccountNotFound(toAccountId));
            }

            // Check currencies match
            if (sourceAccount.Currency != destinationAccount.Currency)
            {
                return Result.Failure(DomainErrors.VirtualAccount.CurrencyMismatch);
            }

            var amountResult = Amount.Create(amountValue);
            if (amountResult.IsFailure)
            {
                return Result.Failure(amountResult.Error);
            }

            // Withdraw from source account
            var withdrawResult = sourceAccount.Withdraw(
                amountResult.Value,
                $"Transfer to {destinationAccount.AccountNumber.Value}: {description}");

            if (withdrawResult.IsFailure)
            {
                return Result.Failure(withdrawResult.Error);
            }

            // Deposit to destination account
            var depositResult = destinationAccount.Deposit(
                amountResult.Value,
                $"Transfer from {sourceAccount.AccountNumber.Value}: {description}");

            if (depositResult.IsFailure)
            {
                return Result.Failure(depositResult.Error);
            }

            // Update both accounts
            await virtualAccountRepository.UpdateAsync(sourceAccount, cancellationToken);
            await virtualAccountRepository.UpdateAsync(destinationAccount, cancellationToken);

            // Commit transaction
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            // Rollback transaction in case of error
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

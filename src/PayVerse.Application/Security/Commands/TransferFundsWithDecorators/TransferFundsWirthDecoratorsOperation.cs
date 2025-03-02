using PayVerse.Domain.Decorators.Security;
using PayVerse.Domain.Exceptions;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Security.Commands.TransferFundsWithDecorators;

public class TransferFundsWirthDecoratorsOperation(
    IVirtualAccountRepository accountRepository,
    IUnitOfWork unitOfWork) : ISecureOperation<TransferFundsWithDecoratorsCommand, TransferFundsWithDecoratorsResult>
{
    public async Task<TransferFundsWithDecoratorsResult> ExecuteAsync(TransferFundsWithDecoratorsCommand request, CancellationToken cancellationToken = default)
    {
        var (sourceAccountId, destinationAccountId, amount, description) = request;

        var sourceAccount = await accountRepository.GetByIdAsync(sourceAccountId, cancellationToken);
        var destinationAccount = await accountRepository.GetByIdAsync(destinationAccountId, cancellationToken);

        if (sourceAccount is null || destinationAccount is null)
        {
            return new TransferFundsWithDecoratorsResult(
                 Guid.Empty,
                 false,
                 "One or both accounts not found"
            );
        }

        try
        {
            var amountResult = Amount.Create(amount).Value;

            // Perform the transfer
            var transferResult = sourceAccount.TransferFunds(
                destinationAccount,
                amountResult);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new TransferFundsWithDecoratorsResult(
                Guid.NewGuid(), // transferResult.TransactionId - fix this coming soon
                true,
                "Transfer completed successfully"
            );
        }
        catch (DomainException ex)
        {
            return new TransferFundsWithDecoratorsResult(
                Guid.Empty,
                false,
                ex.Message
            );
        }
    }
}
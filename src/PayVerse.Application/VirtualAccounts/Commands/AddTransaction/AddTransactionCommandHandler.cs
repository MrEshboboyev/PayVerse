using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.VirtualAccounts.Commands.AddTransaction;

internal sealed class AddTransactionCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    ITransactionRepository transactionRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddTransactionCommand>
{
    public async Task<Result> Handle(
        AddTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var (virtualAccountId, amount, date, description) = request;
        
        #region Get Virtual Account

        var virtualAccount = await virtualAccountRepository.GetByIdAsync(
            virtualAccountId, 
            cancellationToken);
        if (virtualAccount is null)
        {
            return Result.Failure(
                DomainErrors.VirtualAccount.NotFound(virtualAccountId));
        }

        #endregion
        
        #region Prepare value objects
        
        var amountResult = Amount.Create(amount);
        if (amountResult.IsFailure)
        {
            return Result.Failure(
                amountResult.Error);
        }
        
        #endregion

        #region Add Transaction

        var addTransactionResult = virtualAccount.AddTransaction(
            amountResult.Value, 
            date, 
            description);
        if (addTransactionResult.IsFailure)
        {
            return Result.Failure(
                addTransactionResult.Error);
        }

        #endregion

        #region Save Changes

        await transactionRepository.AddAsync(addTransactionResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
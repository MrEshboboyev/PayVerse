using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;

internal sealed class CreateVirtualAccountCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateVirtualAccountCommand>
{
    public async Task<Result> Handle(
        CreateVirtualAccountCommand request,
        CancellationToken cancellationToken)
    {
        var (accountNumber, currencyCode, balance, userId) = request;
        
        #region Prepare value objects
        
        var accountNumberResult = AccountNumber.Create(accountNumber);
        if (accountNumberResult.IsFailure)
        {
            return Result.Failure(
                accountNumberResult.Error);
        }
        
        var currencyResult = Currency.Create(currencyCode);
        if (currencyResult.IsFailure)
        {
            return Result.Failure(
                currencyResult.Error);
        }
        
        var balanceResult = Balance.Create(balance);
        if (balanceResult.IsFailure)
        {
            return Result.Failure(
                balanceResult.Error);
        }
        
        #endregion
        
        #region Create Virtual Account

        var virtualAccount = VirtualAccount.Create(
            Guid.NewGuid(),
            accountNumberResult.Value,
            currencyResult.Value,
            balanceResult.Value,
            userId);

        #endregion

        #region Add Virtual Account to Repository

        await virtualAccountRepository.AddAsync(virtualAccount, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Factories.VirtualAccounts;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.VirtualAccounts;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.VirtualAccounts.Commands.CreateVirtualAccount;

internal sealed class CreateVirtualAccountCommandHandler(
    IVirtualAccountRepository virtualAccountRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateVirtualAccountCommand>
{
    public async Task<Result> Handle(
        CreateVirtualAccountCommand request,
        CancellationToken cancellationToken)
    {
        var (currencyCode, userId) = request;
        
        #region Prepare value objects
        
        var currencyResult = Currency.Create(currencyCode);
        if (currencyResult.IsFailure)
        {
            return Result.Failure(
                currencyResult.Error);
        }
        
        #endregion
        
        #region Create Virtual Account

        var virtualAccount = VirtualAccountFactory.Create(
            userId,
            currencyResult.Value);

        #endregion

        #region Add Virtual Account to Repository

        await virtualAccountRepository.AddAsync(virtualAccount, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
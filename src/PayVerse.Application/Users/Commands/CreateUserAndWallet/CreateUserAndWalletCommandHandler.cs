using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Factories.Users;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Repositories.Wallets;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Users;
using PayVerse.Domain.Errors;
using PayVerse.Application.Abstractions;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Application.Users.Commands.CreateUserAndWallet;

internal sealed class CreateUserAndWalletCommandHandler(
    IUserWalletFactory userWalletFactory,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IWalletRepository walletRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateUserAndWalletCommand>
{
    public async Task<Result> Handle(
        CreateUserAndWalletCommand request,
        CancellationToken cancellationToken)
    {
        var (email, password, firstName, lastName, roleId, currency) = request;

        #region Create Email and checking email unique

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(
                DomainErrors.User.EmailAlreadyInUse);
        }

        #endregion

        #region Prepare value objects for User

        var firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure)
        {
            return Result.Failure(firstNameResult.Error);
        }

        var lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure)
        {
            return Result.Failure(lastNameResult.Error);
        }

        #endregion

        #region Create Password Hash

        var passwordHash = passwordHasher.Hash(password);

        #endregion

        #region Get Role

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
        {
            return Result.Failure<Guid>(
                DomainErrors.Role.NotFound(roleId));
        }

        #endregion

        #region Create User

        var user = userWalletFactory.CreateUser(
            emailResult.Value,
            passwordHash,
            firstNameResult.Value,
            lastNameResult.Value,
            role);

        #endregion

        #region Prepare value objects for Wallet

        var currencyResult = Currency.Create(currency);
        if (currencyResult.IsFailure)
        {
            return Result.Failure(currencyResult.Error);
        }

        #endregion

        #region Create Wallet

        var wallet = userWalletFactory.CreateWallet(
            user, 
            currencyResult.Value);

        #endregion

        #region Save User and Wallet

        await userRepository.AddAsync(user, cancellationToken);
        await walletRepository.AddAsync(wallet, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
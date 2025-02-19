using PayVerse.Application.Abstractions;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Application.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        var (email, password, firstName, lastName, roleId) = request;
        
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

        #region Create Other value objects (FirstName, LastName) 

        var createFirstNameResult = FirstName.Create(firstName);
        if (createFirstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createFirstNameResult.Error);
        }

        var createLastNameResult = LastName.Create(lastName);
        if (createLastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createLastNameResult.Error);
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
        
        #region Create new User
        
        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            createFirstNameResult.Value,
            createLastNameResult.Value,
            role
        );
        
        #endregion

        #region Add and Update database

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success(user.Id);
    }
}
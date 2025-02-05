using InspireEd.Domain.Repositories;
using PayVerse.Application.Abstractions;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Application.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        #region Create Email and checking email unique

        var emailResult = Email.Create(request.Email);
        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(
                DomainErrors.User.EmailAlreadyInUse);
        }

        #endregion

        #region Create Other value objects (FirstName, LastName) 

        var createFirstNameResult = FirstName.Create(request.FirstName);
        if (createFirstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createFirstNameResult.Error);
        }

        var createLastNameResult = LastName.Create(request.LastName);
        if (createLastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createLastNameResult.Error);
        }

        #endregion

        #region Create Password Hash

        var passwordHash = passwordHasher.Hash(request.Password);
        
        #endregion
        
        #region Create new User
        
        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            createFirstNameResult.Value,
            createLastNameResult.Value,
            Role.IndividualUser
        );
        
        #endregion

        #region Add and Update database

        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success(user.Id);
    }
}
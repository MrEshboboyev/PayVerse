using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Application.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var (userId, firstName, lastName) = request;
        
        #region Get this user

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(
                DomainErrors.User.NotFound(userId));
        }
        
        #endregion
        
        #region Prepare value objects

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
        
        #region Update user 

        var updateUserDetailsResult = user.UpdateDetails(
            firstNameResult.Value,
            lastNameResult.Value);
        if (updateUserDetailsResult.IsFailure)
        {
            return Result.Failure(
                updateUserDetailsResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}

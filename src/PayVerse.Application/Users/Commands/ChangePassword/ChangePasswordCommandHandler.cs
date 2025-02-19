using PayVerse.Application.Abstractions;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Application.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(
        ChangePasswordCommand request, 
        CancellationToken cancellationToken)
    {
        var (email, oldPassword, newPassword) = request;
        
        #region Prepare Value Objects

        var createEmailResult = Email.Create(email);
        if (createEmailResult.IsFailure)
        {
            return Result.Failure<string>(
                createEmailResult.Error);
        }

        #endregion

        #region Checking User is null and credentials is valid

        var user = await userRepository.GetByEmailAsync(
            createEmailResult.Value,
            cancellationToken);
        if (user is null || !passwordHasher.Verify(oldPassword, user.PasswordHash))
        {
            return Result.Failure<string>(
                DomainErrors.User.InvalidCredentials);
        }

        #endregion

        #region Change password
        
        var passwordHash = passwordHasher.Hash(newPassword);
        var changePasswordResult = user.ChangePassword(passwordHash);
        if (changePasswordResult.IsFailure)
        {
            return Result.Failure(
                changePasswordResult.Error);
        }
        
        #endregion

        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

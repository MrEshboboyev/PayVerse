using PayVerse.Application.Abstractions;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Abstractions.Security;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Application.Users.Commands.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider, 
    IPasswordHasher passwordHasher) : ICommandHandler<LoginCommand, string>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        #region Prepare Value Objects

        var createEmailResult = Email.Create(request.Email);
        if (createEmailResult.IsFailure)
        {
            return Result.Failure<string>(
                createEmailResult.Error);
        }

        #endregion

        #region Try to Get User by Email.Value

        var user = await userRepository.GetByEmailAsync(
            createEmailResult.Value,
            cancellationToken);

        #endregion

        #region Checking User is null and credentials is valid

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result.Failure<string>(
                DomainErrors.User.InvalidCredentials);
        }

        #endregion

        #region Generate JWT Token using custom provider

        var token = await jwtProvider.GenerateAsync(user);

        #endregion

        return Result.Success(token);
    }
}


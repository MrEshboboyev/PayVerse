using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Commands.EnableTwoFactorAuthentication;

internal sealed class EnableTwoFactorAuthenticationCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<EnableTwoFactorAuthenticationCommand>
{
    public async Task<Result> Handle(
        EnableTwoFactorAuthenticationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        #region Get this user

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(userId));
        }
        
        #endregion
        
        #region Enable two-factor authentication

        var enableTwoFactorAuthenticationResult = user.EnableTwoFactorAuthentication();
        if (enableTwoFactorAuthenticationResult.IsSuccess)
        {
            return Result.Failure(enableTwoFactorAuthenticationResult.Error);
        }
        
        #endregion
        
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

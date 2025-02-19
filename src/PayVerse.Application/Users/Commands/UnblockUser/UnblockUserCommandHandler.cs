using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Commands.UnblockUser;

internal sealed class UnblockUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UnblockUserCommand>
{
    public async Task<Result> Handle(
        UnblockUserCommand request,
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
        
        #region unblock user

        var unblockUserResult = user.Unblock();
        if (unblockUserResult.IsFailure)
        {
            return Result.Failure(unblockUserResult.Error);
        }
        
        #endregion
        
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

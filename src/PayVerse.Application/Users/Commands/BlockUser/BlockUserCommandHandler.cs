using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Commands.BlockUser;

internal sealed class BlockUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<BlockUserCommand>
{
    public async Task<Result> Handle(
        BlockUserCommand request,
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
        
        #region Block user

        var userBlockResult = user.Block();
        if (userBlockResult.IsFailure)
        {
            return Result.Failure(userBlockResult.Error);
        }
        
        #endregion
        
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

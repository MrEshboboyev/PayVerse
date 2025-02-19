using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Commands.RevokeRoleFromUser;

internal sealed class RevokeRoleFromUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RevokeRoleFromUserCommand>
{
    public async Task<Result> Handle(
        RevokeRoleFromUserCommand request, 
        CancellationToken cancellationToken)
    {
        var (userId, roleId) = request;

        #region Get user and role
        
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound(userId));
        }

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
        {
            return Result.Failure(DomainErrors.Role.NotFound(roleId));
        }
        
        #endregion
        
        #region Revoke role

        var removeResult = user.RemoveRole(role);
        if (removeResult.IsFailure)
        {
            return Result.Failure(removeResult.Error);
        }
        
        #endregion

        #region Update database
        
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}

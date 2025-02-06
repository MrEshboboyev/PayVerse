using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Factories;
using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Queries.GetUserWithRolesById;

internal sealed class GetUserWithRolesByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserWithRolesByIdQuery, UserWithRolesResponse>
{
    public async Task<Result<UserWithRolesResponse>> Handle(
        GetUserWithRolesByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        // Fetch the user from the repository
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        // If user is not found, return a failure result
        if (user is null)
        {
            return Result.Failure<UserWithRolesResponse>(
                DomainErrors.User.NotFound(userId));
        }

        #region Prepare response

        var response = new UserWithRolesResponse(
            UserResponseFactory.Create(user),
                user.Roles
                    .Select(RoleResponseFactory.Create)
                    .ToList()
                    .AsReadOnly()
        );

        #endregion

        return Result.Success(response);
    }
}
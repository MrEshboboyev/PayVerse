using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Factories;
using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Queries.GetAllUsers;

internal sealed class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetAllUsersQuery, UserListResponse>
{
    public async Task<Result<UserListResponse>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        // Fetch all users from the repository
        var users = await userRepository.GetAllAsync(cancellationToken);

        #region Prepare response
        
        var responses = new UserListResponse(
            users
                .Select(UserResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        #endregion

        return Result.Success(responses);
    }
}

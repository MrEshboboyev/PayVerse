using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Factories;
using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Queries.SearchUsers;

internal sealed class SearchUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<SearchUsersQuery, UserListResponse>
{
    public async Task<Result<UserListResponse>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken)
    {
        var (email, name, roleId) = request;
        
        var users = await userRepository.SearchAsync(
            email,
            name,
            roleId,
            cancellationToken);
        
        var response = new UserListResponse(
            users
                .Select(UserResponseFactory.Create)
                .ToList());
        
        return Result.Success(response);
    }
}

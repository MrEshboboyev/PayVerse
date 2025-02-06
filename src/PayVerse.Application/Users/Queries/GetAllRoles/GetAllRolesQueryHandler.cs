using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Factories;
using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Application.Users.Queries.GetAllUsers;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Users.Queries.GetAllRoles;

internal sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetAllRolesQuery, RoleListResponse>
{
    public async Task<Result<RoleListResponse>> Handle(
        GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        // Fetch all users from the repository
        var users = await roleRepository.GetAllAsync(cancellationToken);

        #region Prepare response
        
        var responses = new RoleListResponse(
            users
                .Select(RoleResponseFactory.Create)
                .ToList()
                .AsReadOnly());
        
        #endregion

        return Result.Success(responses);
    }
}

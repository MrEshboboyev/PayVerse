using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Domain.Entities.Users;

namespace PayVerse.Application.Users.Queries.Common.Factories;

public static class RoleResponseFactory
{
    public static RoleResponse Create(Role role)
    {
        return new RoleResponse(
            role.Id,
            role.Name);
    }
}
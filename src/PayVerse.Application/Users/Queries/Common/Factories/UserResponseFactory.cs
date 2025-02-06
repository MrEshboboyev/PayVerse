using PayVerse.Application.Users.Queries.Common.Responses;
using PayVerse.Domain.Entities.Users;

namespace PayVerse.Application.Users.Queries.Common.Factories;

public static class UserResponseFactory
{
    public static UserResponse Create(User user)
    {
        return new UserResponse(
            user.Id,
            user.Email.Value,
            user.FirstName.Value,
            user.LastName.Value);
    }
}
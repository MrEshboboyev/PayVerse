using PayVerse.Application.Abstractions.Messaging;

namespace PayVerse.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
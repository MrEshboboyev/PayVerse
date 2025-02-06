using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common;
using PayVerse.Application.Users.Queries.Common.Responses;

namespace PayVerse.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Responses;

namespace PayVerse.Application.Users.Queries.GetUserWithRolesById;

public sealed record GetUserWithRolesByIdQuery(Guid UserId) : IQuery<UserWithRolesResponse>;
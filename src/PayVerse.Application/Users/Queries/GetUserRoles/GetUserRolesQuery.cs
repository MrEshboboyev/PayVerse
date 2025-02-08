using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Responses;

namespace PayVerse.Application.Users.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(Guid UserId) : IQuery<RoleListResponse>;

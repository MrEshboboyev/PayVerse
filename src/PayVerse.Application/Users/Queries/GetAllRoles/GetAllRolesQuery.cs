using PayVerse.Application.Abstractions.Messaging;
using PayVerse.Application.Users.Queries.Common.Responses;

namespace PayVerse.Application.Users.Queries.GetAllRoles;

public sealed record GetAllRolesQuery() : IQuery<RoleListResponse>;
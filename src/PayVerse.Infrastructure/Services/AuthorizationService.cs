using Microsoft.Extensions.Logging;
using PayVerse.Application.Common.Interfaces;
using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.Repositories.Users;

namespace PayVerse.Infrastructure.Services;

/// <summary>
/// Implementation of authorization service
/// </summary>
public class AuthorizationService(
    IUserRepository userRepository,
    //IPermissionRepository permissionRepository,
    //IRolePermissionRepository rolePermissionRepository,
    //IResourceAccessRepository resourceAccessRepository,
    ILogger<AuthorizationService> logger) : IAuthorizationService
{
    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            logger.LogWarning("User {UserId} not found when getting roles", userId);
            return Enumerable.Empty<string>();
        }

        return user.Roles.Select(r => r.Name).ToList();
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            logger.LogWarning("Attempt to check null or empty role for user {UserId}", userId);
            return false;
        }

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            logger.LogWarning("User {UserId} not found when checking role {Role}", userId, role);
            return false;
        }

        return user.Roles.Any(r => r.Name == role);
    }

    //public async Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default)
    //{
    //    if (string.IsNullOrWhiteSpace(permission))
    //    {
    //        logger.LogWarning("Attempt to check null or empty permission for user {UserId}", userId);
    //        return false;
    //    }

    //    var user = await userRepository.GetByIdAsync(userId, cancellationToken);

    //    if (user == null)
    //    {
    //        logger.LogWarning("User {UserId} not found when checking permission {Permission}", userId, permission);
    //        return false;
    //    }

    //    // Admin role has all permissions
    //    if (user.Roles.Any(r => r.Name == Role.Admin.Name))
    //    {
    //        return true;
    //    }

    //    // Get all permissions for the user's roles
    //    var userRoleIds = user.Roles.Select(r => r.Id).ToList();
    //    var rolePermissions = await rolePermissionRepository.GetPermissionsByRoleIdsAsync(userRoleIds, cancellationToken);

    //    return rolePermissions.Any(p => p.Name == permission);
    //}

    //public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    //{
    //    var user = await userRepository.GetByIdAsync(userId, cancellationToken);

    //    if (user == null)
    //    {
    //        logger.LogWarning("User {UserId} not found when getting permissions", userId);
    //        return Enumerable.Empty<string>();
    //    }

    //    // Admin role has all permissions
    //    if (user.Roles.Any(r => r.Name == Role.Admin.Name))
    //    {
    //        // Return all available permissions
    //        var allPermissions = await permissionRepository.GetAllAsync(cancellationToken);
    //        return allPermissions.Select(p => p.Name).ToList();
    //    }

    //    // Get permissions for the user's roles
    //    var userRoleIds = user.Roles.Select(r => r.Id).ToList();
    //    var rolePermissions = await rolePermissionRepository.GetPermissionsByRoleIdsAsync(userRoleIds, cancellationToken);

    //    return rolePermissions.Select(p => p.Name).Distinct().ToList();
    //}

    //public async Task<bool> CanAccessResourceAsync(Guid userId, string resourceType, Guid resourceId, string action, CancellationToken cancellationToken = default)
    //{
    //    if (string.IsNullOrWhiteSpace(resourceType) || string.IsNullOrWhiteSpace(action))
    //    {
    //        logger.LogWarning("Invalid resource type or action for user {UserId}", userId);
    //        return false;
    //    }

    //    var user = await userRepository.GetByIdAsync(userId, cancellationToken);

    //    if (user == null)
    //    {
    //        logger.LogWarning("User {UserId} not found when checking resource access", userId);
    //        return false;
    //    }

    //    // Admin role has access to all resources
    //    if (user.Roles.Any(r => r.Name == Role.Admin.Name))
    //    {
    //        return true;
    //    }

    //    //// Check for resource-specific permissions
    //    //var hasAccess = await resourceAccessRepository.HasAccessAsync(
    //    //    userId, resourceType, resourceId, action, cancellationToken);

    //    //if (hasAccess)
    //    //{
    //    //    return true;
    //    //}

    //    // Check if the user has a role with the required permission for this action on this resource type
    //    var permissionName = $"{resourceType}:{action}";
    //    return await HasPermissionAsync(userId, permissionName, cancellationToken);
    //}
}
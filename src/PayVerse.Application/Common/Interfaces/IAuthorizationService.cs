namespace PayVerse.Application.Common.Interfaces;

/// <summary>
/// Service for authorization and permission checks
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Gets all roles for a user
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of roles the user has</returns>
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a user has a specific role
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <param name="role">Role to check</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the user has the role, false otherwise</returns>
    Task<bool> IsInRoleAsync(Guid userId, string role, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Checks if a user has a specific permission
    ///// </summary>
    ///// <param name="userId">ID of the user</param>
    ///// <param name="permission">Permission to check</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>True if the user has the permission, false otherwise</returns>
    //Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Gets all permissions for a user
    ///// </summary>
    ///// <param name="userId">ID of the user</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>List of permissions the user has</returns>
    //Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Checks if a user has access to a specific resource
    ///// </summary>
    ///// <param name="userId">ID of the user</param>
    ///// <param name="resourceType">Type of resource</param>
    ///// <param name="resourceId">ID of the resource</param>
    ///// <param name="action">Action to perform on the resource</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>True if the user has access to the resource, false otherwise</returns>
    //Task<bool> CanAccessResourceAsync(Guid userId, string resourceType, Guid resourceId, string action, CancellationToken cancellationToken = default);
}
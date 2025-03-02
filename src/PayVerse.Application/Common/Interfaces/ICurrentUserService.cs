namespace PayVerse.Application.Common.Interfaces;

/// <summary>
/// Service for getting information about the currently authenticated user
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// The ID of the currently authenticated user
    /// </summary>
    Guid UserId { get; }

    /// <summary>
    /// The ID of the currently authenticated user
    /// </summary>
    string IpAddress { get; }

    /// <summary>
    /// The ID of the currently authenticated user
    /// </summary>
    string DeviceInfo { get; }

    /// <summary>
    /// The ID of the currently authenticated user
    /// </summary>
    string AuthToken { get; }

    /// <summary>
    /// The email of the currently authenticated user
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Checks if the current user is authenticated
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Checks if the current user has the specified role
    /// </summary>
    /// <param name="roleName">Name of the role to check</param>
    /// <returns>True if the user has the role, otherwise false</returns>
    bool IsInRole(string roleName);
}

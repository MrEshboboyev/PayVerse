using PayVerse.Domain.Entities.Users;

namespace PayVerse.Application.Common.Interfaces;

/// <summary>
/// Service for user authentication
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user with email and password
    /// </summary>
    /// <param name="email">User's email</param>
    /// <param name="password">User's password</param>
    /// <param name="ipAddress">IP address of the request</param>
    /// <param name="deviceInfo">Information about the device</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result with user information and token</returns>
    Task<(bool Success, string Token, User User)> AuthenticateAsync(
        string email,
        string password,
        string ipAddress,
        string deviceInfo,
        CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Refreshes an authentication token
    ///// </summary>
    ///// <param name="refreshToken">Refresh token</param>
    ///// <param name="ipAddress">IP address of the request</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>New authentication token</returns>
    //Task<(bool Success, string Token)> RefreshTokenAsync(
    //    string refreshToken,
    //    string ipAddress,
    //    CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Validates an authentication token
    ///// </summary>
    ///// <param name="token">Token to validate</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>True if the token is valid, false otherwise</returns>
    //Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Logs out a user
    ///// </summary>
    ///// <param name="userId">ID of the user</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>Task representing the asynchronous operation</returns>
    //Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Generates a password reset token for a user
    ///// </summary>
    ///// <param name="email">User's email</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>Password reset token</returns>
    //Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Resets a user's password using a reset token
    ///// </summary>
    ///// <param name="email">User's email</param>
    ///// <param name="token">Password reset token</param>
    ///// <param name="newPassword">New password</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>True if the password was reset successfully, false otherwise</returns>
    //Task<bool> ResetPasswordAsync(
    //    string email,
    //    string token,
    //    string newPassword,
    //    CancellationToken cancellationToken = default);
}
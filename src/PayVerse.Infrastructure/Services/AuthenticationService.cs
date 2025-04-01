using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayVerse.Application.Abstractions;
using PayVerse.Application.Abstractions.Security;
using PayVerse.Application.Common.Interfaces;
using PayVerse.Application.Common.Interfaces.Security;
using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.Enums.Security;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Infrastructure.Services;

/// <summary>
/// Implementation of authentication service
/// </summary>
public class AuthenticationService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    IAuditLogService auditLogService,
    ISecurityIncidentService securityIncidentService,
    ILogger<AuthenticationService> logger) : IAuthenticationService
{
    public async Task<(bool Success, string Token, User User)> AuthenticateAsync(
        string email,
        string password,
        string ipAddress,
        string deviceInfo,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            logger.LogWarning("Authentication attempt with empty email or password from IP {IpAddress}", ipAddress);
            return (false, null, null);
        }

        var emailObj = Email.Create(email);
        var user = await userRepository.GetByEmailAsync(emailObj.Value, cancellationToken);

        if (user == null)
        {
            logger.LogWarning("Authentication attempt for non-existent user {Email} from IP {IpAddress}", email, ipAddress);
            await securityIncidentService.LogIncidentAsync(
                SecurityIncidentType.FailedLogin,
                $"Failed login attempt for non-existent user {email}",
                null,
                ipAddress,
                cancellationToken);
            return (false, null, null);
        }

        if (user.IsBlocked)
        {
            logger.LogWarning("Authentication attempt for blocked user {UserId} from IP {IpAddress}", user.Id, ipAddress);
            await securityIncidentService.LogIncidentAsync(
                SecurityIncidentType.BlockedUserLoginAttempt,
                $"Blocked user {user.Id} attempted to login",
                user.Id,
                ipAddress,
                cancellationToken);
            return (false, null, null);
        }

        if (!passwordHasher.Verify(password, user.PasswordHash))
        {
            logger.LogWarning("Failed authentication attempt for user {UserId} from IP {IpAddress}", user.Id, ipAddress);
            await securityIncidentService.LogIncidentAsync(
                SecurityIncidentType.FailedLogin,
                $"Failed login attempt for user {user.Id}",
                user.Id,
                ipAddress,
                cancellationToken);
            return (false, null, null);
        }

        // Generate JWT token
        var token = await jwtProvider.GenerateAsync(user);

        //// Generate refresh token
        //var refreshToken = GenerateRefreshToken(user.Id, ipAddress);
        //await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        // Log successful login
        await auditLogService.CreateAuditLogAsync(
            user.Id,
            "UserLoggedIn",
            $"User logged in from IP {ipAddress}",
            ipAddress,
            deviceInfo,
            cancellationToken);

        logger.LogInformation("User {UserId} successfully authenticated from IP {IpAddress}", user.Id, ipAddress);

        return (true, token, user);
    }

    //public async Task<(bool Success, string Token)> RefreshTokenAsync(
    //    string refreshToken,
    //    string ipAddress,
    //    CancellationToken cancellationToken = default)
    //{
    //    if (string.IsNullOrWhiteSpace(refreshToken))
    //    {
    //        logger.LogWarning("Refresh token attempt with empty token from IP {IpAddress}", ipAddress);
    //        return (false, null);
    //    }

    //    //var token = await refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);

    //    //if (token == null)
    //    //{
    //    //    logger.LogWarning("Refresh token attempt with invalid token from IP {IpAddress}", ipAddress);
    //    //    await securityIncidentService.LogIncidentAsync(
    //    //        SecurityIncidentType.InvalidRefreshToken,
    //    //        "Invalid refresh token used",
    //    //        null,
    //    //        ipAddress,
    //    //        cancellationToken);
    //    //    return (false, null);
    //    //}

    //    if (token.ExpiryDate < DateTime.UtcNow)
    //    {
    //        logger.LogWarning("Expired refresh token used by user {UserId} from IP {IpAddress}", token.UserId, ipAddress);
    //        await securityIncidentService.LogIncidentAsync(
    //            SecurityIncidentType.ExpiredRefreshToken,
    //            "Expired refresh token used",
    //            token.UserId,
    //            ipAddress,
    //            cancellationToken);
    //        return (false, null);
    //    }

    //    var user = await userRepository.GetByIdAsync(token.UserId, cancellationToken);

    //    if (user == null || user.IsBlocked)
    //    {
    //        logger.LogWarning("Refresh token used for non-existent or blocked user from IP {IpAddress}", ipAddress);
    //        await securityIncidentService.LogIncidentAsync(
    //            SecurityIncidentType.InvalidRefreshToken,
    //            "Refresh token used for invalid user",
    //            token.UserId,
    //            ipAddress,
    //            cancellationToken);
    //        return (false, null);
    //    }

    //    // Generate new JWT token
    //    var newJwtToken = GenerateJwtToken(user);

    //    // Revoke the old refresh token and generate a new one
    //    await refreshTokenRepository.RemoveAsync(token.Id, cancellationToken);

    //    var newRefreshToken = GenerateRefreshToken(user.Id, ipAddress);
    //    await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

    //    logger.LogInformation("Refresh token successfully used for user {UserId} from IP {IpAddress}", user.Id, ipAddress);

    //    return (true, newJwtToken);
    //}

    //public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    //{
    //    if (string.IsNullOrWhiteSpace(token))
    //    {
    //        logger.LogWarning("Token validation attempt with empty token");
    //        return false;
    //    }

    //    // Check if token is blacklisted
    //    var isBlacklisted = await tokenBlacklistRepository.IsTokenBlacklistedAsync(token, cancellationToken);

    //    if (isBlacklisted)
    //    {
    //        logger.LogWarning("Blacklisted token used");
    //        return false;
    //    }

    //    try
    //    {
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);

    //        tokenHandler.ValidateToken(token, new TokenValidationParameters
    //        {
    //            ValidateIssuerSigningKey = true,
    //            IssuerSigningKey = new SymmetricSecurityKey(key),
    //            ValidateIssuer = true,
    //            ValidIssuer = configuration["Jwt:Issuer"],
    //            ValidateAudience = true,
    //            ValidAudience = configuration["Jwt:Audience"],
    //            ValidateLifetime = true,
    //            ClockSkew = TimeSpan.Zero
    //        }, out SecurityToken validatedToken);

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        logger.LogWarning(ex, "Token validation failed");
    //        return false;
    //    }
    //}

    //public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default)
    //{
    //    // Invalidate all refresh tokens for the user
    //    await refreshTokenRepository.RemoveAllForUserAsync(userId, cancellationToken);

    //    logger.LogInformation("User {UserId} logged out", userId);
    //}

    //public async Task<string> GeneratePasswordResetTokenAsync(string email, CancellationToken cancellationToken = default)
    //{
    //    if (string.IsNullOrWhiteSpace(email))
    //    {
    //        logger.LogWarning("Password reset attempt with empty email");
    //        return null;
    //    }

    //    var emailObj =
}
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PayVerse.Application.Abstractions.Security;
using PayVerse.Domain.Entities.Users;

namespace PayVerse.Infrastructure.Authentication;

/// <summary>
/// Generates JWT tokens for authenticated users.
/// </summary>
internal sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;

    /// <summary>
    /// Generates a JWT token for the given user.
    /// </summary>
    public async Task<string> GenerateAsync(User user)
    {
        var claims = new List<Claim>
        {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email.Value)
        };

        #region Add Roles
        
        // Add roles
        claims.AddRange(user.Roles.Select(
            role => new Claim(ClaimTypes.Role, role.Name)));

        #endregion

        var signingCredentials = new SigningCredentials(
             new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(_options.SecretKey)),
             SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler()
             .WriteToken(token);

        return tokenValue;
    }
}


using Microsoft.AspNetCore.Http;
using PayVerse.Application.Common.Interfaces;
using System.Security.Claims;

namespace PayVerse.Infrastructure.Services;

/// <summary>
/// Implementation of current user service using HTTP context
/// </summary>
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId);
        }
    }

    public string Email
    {
        get
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }

    public string IpAddress => throw new NotImplementedException();

    public string DeviceInfo => throw new NotImplementedException();

    public string AuthToken => throw new NotImplementedException();

    public bool IsInRole(string roleName)
    {
        return httpContextAccessor.HttpContext?.User?.IsInRole(roleName) ?? false;
    }
}

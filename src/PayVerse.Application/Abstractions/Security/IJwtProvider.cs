using PayVerse.Domain.Entities.Users;

namespace PayVerse.Application.Abstractions.Security;

public interface IJwtProvider
{
    Task<string> GenerateAsync(User user);
}

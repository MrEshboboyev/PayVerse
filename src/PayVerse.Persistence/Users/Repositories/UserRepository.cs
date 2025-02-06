using PayVerse.Domain.Entities;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Persistence.Users.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<User>()
            .ToListAsync(cancellationToken);

    public async Task<User> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<User>()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<User> GetByIdWithRolesAsync(
        Guid id,
        CancellationToken cancellationToken = default)
        => await dbContext
            .Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<User> GetByEmailAsync(
        Email email,
        CancellationToken cancellationToken = default) =>
        await dbContext
            .Set<User>()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

    public async Task<bool> IsEmailUniqueAsync(
        Email email,
        CancellationToken cancellationToken = default) =>
        !await dbContext
            .Set<User>()
            .AnyAsync(user => user.Email == email, cancellationToken);

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Set<User>()
            .ToListAsync(cancellationToken);

    public void Add(User user) =>
        dbContext.Set<User>().Add(user);

    public void Update(User user) =>
        dbContext.Set<User>().Update(user);

    public void Delete(User user)
        => dbContext.Set<User>()
            .Remove(user);
}
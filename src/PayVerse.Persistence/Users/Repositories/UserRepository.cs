using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.Repositories.Users;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Persistence.Users.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<IEnumerable<User>> SearchAsync(
        string email,
        string name,
        int? roleId,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<User>().AsQueryable();

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(user => user.Email.Value == email);
        }

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(user => user.FirstName.Value == name 
                                        || user.LastName.Value == name);
        }

        if (roleId.HasValue)
        {
            query = query
                .Include(user => user.Roles)
                .Where(user => user.Roles.Any(role => role.Id == roleId));
        }

        return await query.ToListAsync(cancellationToken);
    }
    
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
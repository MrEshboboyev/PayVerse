using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Repositories.Notifications;

namespace PayVerse.Persistence.Notifications.Repositories;

public sealed class NotificationRepository(ApplicationDbContext dbContext) : INotificationRepository
{
    public Task<List<Notification>> GetAllAsync(CancellationToken cancellationToken)
    {
        return dbContext.Set<Notification>().ToListAsync(cancellationToken);
    }

    public async Task<Notification> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Notification>()
            .FirstOrDefaultAsync(notification => notification.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Notification>()
            .Where(notification => notification.UserId == userId && !notification.IsRead)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Notification>()
            .Where(notification => notification.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        await dbContext.Set<Notification>().AddAsync(notification, cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken)
    {
        dbContext.Set<Notification>().Update(notification);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

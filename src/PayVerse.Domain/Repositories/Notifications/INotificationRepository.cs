using PayVerse.Domain.Entities.Notifications;

namespace PayVerse.Domain.Repositories.Notifications;

public interface INotificationRepository : IRepository<Notification>
{
    Task<Notification> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Notification notification, CancellationToken cancellationToken);
    Task UpdateAsync(Notification notification, CancellationToken cancellationToken);
    Task<List<Notification>> GetAllAsync(CancellationToken cancellationToken);
}
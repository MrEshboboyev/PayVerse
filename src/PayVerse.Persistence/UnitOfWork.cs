using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Repositories;
using PayVerse.Persistence.Outbox;
using System.Data;

namespace PayVerse.Persistence;

/// <summary> 
/// Implements the unit of work pattern, managing the application database context. 
/// </summary>
internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction _currentTransaction;

    /// <summary> 
    /// Saves changes to the database asynchronously. 
    /// </summary> 
    /// <param name="cancellationToken">Cancellation token.</param> 
    /// <returns>Task representing the save operation.</returns>
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();
        UpdateAuditableEntities();
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary> 
    /// Begins a new database transaction. 
    /// </summary> 
    /// <returns>IDbTransaction representing the transaction.</returns>
    public IDbTransaction BeginTransaction()
    {
        var transaction = dbContext.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }

    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the transaction.</returns>
    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress");
        }

        _currentTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return _currentTransaction.GetDbTransaction();
    }

    /// <summary>
    /// Commits the current transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the commit operation.</returns>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No active transaction to commit");
        }

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await _currentTransaction.CommitAsync(cancellationToken);
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    /// <summary>
    /// Rolls back the current transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the rollback operation.</returns>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            throw new InvalidOperationException("No active transaction to roll back");
        }

        try
        {
            await _currentTransaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }

    #region Private Methods

    /// <summary> 
    /// Converts domain events to outbox messages for reliable event processing. 
    /// </summary>
    private void ConvertDomainEventsToOutboxMessages()
    {
        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
            })
            .ToList();
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    /// <summary> 
    /// Updates auditable entities with creation and modification timestamps. 
    /// </summary>
    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
           dbContext
              .ChangeTracker
              .Entries<IAuditableEntity>();
        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
            }
            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
            }
        }
    }

    #endregion
}

using System.Data;

namespace PayVerse.Domain.Repositories;

/// <summary> 
/// Defines the contract for a unit of work pattern. 
/// </summary>
public interface IUnitOfWork
{
    /// <summary> 
    /// Saves changes to the database asynchronously. 
    /// </summary> 
    /// <param name="cancellationToken">Cancellation token.</param> 
    /// <returns>Task representing the save operation.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary> 
    /// Begins a new database transaction. 
    /// </summary> 
    /// <returns>IDbTransaction representing the transaction.</returns>
    IDbTransaction BeginTransaction();


    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the transaction.</returns>
    Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the commit operation.</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Task representing the rollback operation.</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}


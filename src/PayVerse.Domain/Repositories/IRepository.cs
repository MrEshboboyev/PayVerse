using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Repositories;

/// <summary>
/// Defines the repository interface for aggregate roots.
/// </summary>
/// <typeparam name="T">The type of the aggregate root.</typeparam>
public interface IRepository<T>
    where T : AggregateRoot
{
}
namespace PayVerse.Domain.Builders;

/// <summary>
/// Generic builder interface for domain entities
/// </summary>
public interface IBuilder<T>
{
    T Build();
}

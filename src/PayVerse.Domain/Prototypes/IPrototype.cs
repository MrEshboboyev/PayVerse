namespace PayVerse.Domain.Prototypes;

/// <summary>
/// Interface for the Prototype pattern that defines cloning operations
/// </summary>
public interface IPrototype<T>
{
    /// <summary>
    /// Creates a shallow copy of the current object
    /// </summary>
    T ShallowCopy();

    /// <summary>
    /// Creates a deep copy of the current object
    /// </summary>
    T DeepCopy();
}
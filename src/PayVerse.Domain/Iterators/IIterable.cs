namespace PayVerse.Domain.Iterators;

/// <summary>
/// Interface for collections that can be iterated
/// </summary>
/// <typeparam name="T">Type of elements in the collection</typeparam>
public interface IIterable<T>
{
    IIterator<T> CreateIterator();
}
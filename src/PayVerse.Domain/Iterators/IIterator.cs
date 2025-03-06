namespace PayVerse.Domain.Iterators;


/// <summary>
/// Generic interface for Iterator pattern
/// </summary>
/// <typeparam name="T">Type of elements to iterate</typeparam>
public interface IIterator<T>
{
    bool HasNext();
    T Next();
    void Reset();
    T Current();
}

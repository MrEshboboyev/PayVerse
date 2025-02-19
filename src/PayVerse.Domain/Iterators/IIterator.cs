namespace PayVerse.Domain.Iterators;

// ✅ Supports pagination without exposing internal transaction details.

public interface IIterator<T>
{
    bool HasNext();
    T Next();
}

using PayVerse.Domain.Iterators;

namespace PayVerse.Domain.Collections;

public interface IAggregate<T>
{
    IIterator<T> CreateIterator();
}
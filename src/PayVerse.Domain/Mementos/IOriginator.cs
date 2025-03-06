using PayVerse.Domain.Shared;

namespace PayVerse.Domain.Mementos;

/// <summary>
/// Interface for objects that can create mementos
/// </summary>
public interface IOriginator<T> where T : IMemento
{
    T CreateMemento(string metadata = "");
    Result RestoreFromMemento(T memento);
}

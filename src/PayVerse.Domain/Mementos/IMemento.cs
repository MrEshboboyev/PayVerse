namespace PayVerse.Domain.Mementos;


/// <summary>
/// Interface for objects that can be stored as mementos
/// </summary>
public interface IMemento
{
    DateTime CreatedAt { get; }
    string GetState();
    string GetMetadata();
}
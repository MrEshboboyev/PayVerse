namespace PayVerse.Domain.Mementos;

/// <summary>
/// Caretaker class responsible for memento storage and management
/// </summary>
public class MementoCaretaker<T> where T : IMemento
{
    private readonly Dictionary<Guid, List<T>> _mementos = [];

    public Guid SaveMemento(Guid entityId, T memento)
    {
        if (!_mementos.TryGetValue(entityId, out List<T> value))
        {
            value = [];
            _mementos[entityId] = value;
        }

        value.Add(memento);
        return entityId;
    }

    public T GetMemento(Guid entityId, int index)
    {
        if (!_mementos.TryGetValue(entityId, out List<T> value) || 
            index < 0 || 
            index >= value.Count)
        {
            throw new ArgumentException($"Memento not found for entity {entityId} at index {index}");
        }

        return value[index];
    }

    public T GetLatestMemento(Guid entityId)
    {
        if (!_mementos.TryGetValue(entityId, out List<T> value) || value.Count == 0)
        {
            throw new ArgumentException($"No mementos found for entity {entityId}");
        }

        return value[^1]; // Return the last item
    }

    public List<T> GetAllMementos(Guid entityId)
    {
        if (!_mementos.TryGetValue(entityId, out List<T> value))
        {
            return [];
        }

        return [.. value];
    }

    public bool HasMementos(Guid entityId)
    {
        return _mementos.ContainsKey(entityId) && _mementos[entityId].Count > 0;
    }

    public int GetMementoCount(Guid entityId)
    {
        if (!_mementos.TryGetValue(entityId, out List<T> value))
        {
            return 0;
        }

        return value.Count;
    }
}

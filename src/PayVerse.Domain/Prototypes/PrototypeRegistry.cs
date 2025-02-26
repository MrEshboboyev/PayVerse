namespace PayVerse.Domain.Prototypes;

/// <summary>
/// Registry that manages prototypes for the application
/// </summary>
public class PrototypeRegistry
{
    private readonly Dictionary<string, IPrototype<PrototypeAggregateRoot>> _prototypes = [];

    public void RegisterPrototype(string key, PrototypeAggregateRoot prototype)
    {
        _prototypes[key] = prototype;
    }

    public PrototypeAggregateRoot GetPrototype(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
        {
            throw new KeyNotFoundException($"Prototype with key '{key}' not found in registry");
        }

        return prototype.DeepCopy() as PrototypeAggregateRoot;
    }

    public bool ContainsPrototype(string key)
    {
        return _prototypes.ContainsKey(key);
    }

    public void RemovePrototype(string key)
    {
        _prototypes.Remove(key);
    }
}
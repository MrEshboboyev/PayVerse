using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Prototypes;

/// <summary>
/// Base class for aggregate roots that implements the Prototype pattern
/// </summary>
public abstract class PrototypeAggregateRoot(Guid id) : AggregateRoot(id), IPrototype<PrototypeAggregateRoot>
{
    public abstract PrototypeAggregateRoot ShallowCopy();
    public abstract PrototypeAggregateRoot DeepCopy();
}
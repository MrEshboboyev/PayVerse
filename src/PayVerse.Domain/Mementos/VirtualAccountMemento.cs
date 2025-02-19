using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.VirtualAccounts;

namespace PayVerse.Domain.Mementos;

public class VirtualAccountMemento(Guid id, Balance balance)
{
    public Guid Id { get; } = id;
    public decimal Balance { get; } = balance.Value; // Assuming Amount has a Value property
}
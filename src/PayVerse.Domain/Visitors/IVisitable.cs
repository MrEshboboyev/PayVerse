namespace PayVerse.Domain.Visitors;

/// <summary>
/// Defines the interface for objects that can be visited by visitors.
/// </summary>
public interface IVisitable
{
    void Accept(IVisitor visitor);
}
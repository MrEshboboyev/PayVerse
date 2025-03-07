namespace PayVerse.Domain.Observers;


/// <summary>
/// Subject interface that objects can implement to notify observers of state changes
/// </summary>
public interface ISubject
{
    Task AttachAsync(IObserver observer);
    Task DetachAsync(IObserver observer);
    Task NotifyAsync();
}
namespace PayVerse.Domain.Observers;


/// <summary>
/// Observer interface that objects can implement to receive notifications from subjects
/// </summary>
public interface IObserver
{
    Task UpdateAsync(ISubject subject);
}
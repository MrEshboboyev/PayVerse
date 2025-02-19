using PayVerse.Domain.Mementos;

namespace PayVerse.Application.Memento;

public class AccountCaretaker
{
    private readonly Stack<VirtualAccountMemento> _mementos = new();

    public void SaveState(VirtualAccountMemento memento)
    {
        _mementos.Push(memento);
    }

    public VirtualAccountMemento? Undo()
    {
        if (_mementos.Count != 0)
        {
            return _mementos.Pop();
        }
        return null; // No states to undo
    }

    public bool CanUndo => _mementos.Count != 0;
}
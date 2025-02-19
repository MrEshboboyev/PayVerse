using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.States;

public class ActiveState : IVirtualAccountState
{
    public void Handle(VirtualAccount account)
    {
        Console.WriteLine("✅ Virtual Account is Active.");
    }
}
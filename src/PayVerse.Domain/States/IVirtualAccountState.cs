using PayVerse.Domain.Entities.VirtualAccounts;

namespace PayVerse.Domain.States;

// ✅ Allows dynamic behavior switching for accounts.

public interface IVirtualAccountState
{
    void Handle(VirtualAccount account);
}

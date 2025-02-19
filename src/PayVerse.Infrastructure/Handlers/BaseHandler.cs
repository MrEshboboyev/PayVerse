using PayVerse.Domain.Entities.VirtualAccounts;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects;

namespace PayVerse.Infrastructure.Handlers;

public abstract class BaseHandler : ITransactionHandler
{
    protected ITransactionHandler? NextHandler;

    public void SetNext(ITransactionHandler next)
    {
        NextHandler = next;
    }

    public virtual Result Handle(VirtualAccount account, Amount amount)
    {
        return NextHandler?.Handle(account, amount) ?? Result.Success();
    }
}
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Events.Payments;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Payments;

namespace PayVerse.Domain.Entities.Payments;

/// <summary>
/// Represents a payment in the system.
/// </summary>
public sealed class Payment : AggregateRoot, IAuditableEntity
{
    #region Constructor
    
    private Payment(
        Guid id,
        PaymentAmount amount,
        PaymentStatus status,
        Guid userId)
        : base(id)
    {
        Amount = amount;
        Status = status;
        UserId = userId;

        RaiseDomainEvent(new PaymentInitiatedDomainEvent(
            Guid.NewGuid(),
            id));
    }
    
    #endregion

    #region Properties
    
    public PaymentAmount Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    #endregion

    #region Factory Method
    
    public static Payment Create(
        Guid id,
        PaymentAmount amount,
        PaymentStatus status,
        Guid userId)
    {
        return new Payment(id, amount, status, userId);
    }
    
    #endregion
    
    #region Own methods

    public Result UpdateStatus(PaymentStatus newStatus)
    {
        var oldStatus = Status;
        
        // Status locking or checking with guards (coming soon)
        
        #region Update status
        
        Status = newStatus;
        
        #endregion
        
        #region Domain Events
        
        RaiseDomainEvent(new PaymentStatusUpdatedDomainEvent(
            Guid.NewGuid(),
            Id,
            oldStatus,
            newStatus));
        
        #endregion
        
        return Result.Success();
    }
    
    #endregion
}
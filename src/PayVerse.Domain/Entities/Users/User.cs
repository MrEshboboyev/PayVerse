using PayVerse.Domain.Errors;
using PayVerse.Domain.Events.Users;
using PayVerse.Domain.Primitives;
using PayVerse.Domain.Shared;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Domain.Entities.Users;

/// <summary>
/// Represents a user in the system.
/// </summary>
public sealed class User : AggregateRoot, IAuditableEntity
{
    #region Private fields
    
    private readonly List<Role> _roles = [];
    
    #endregion
    
    #region Constructors

    private User(
        Guid id,
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;

        #region Domain Events

        RaiseDomainEvent(new UserCreatedDomainEvent(
            Guid.NewGuid(),
            id,
            email.Value));

        #endregion
    }

    #endregion

    #region Properties

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new user instance.
    /// </summary>
    public static User Create(
        Guid id,
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName,
        Role role)
    {
        var user = new User(id, email, passwordHash, firstName, lastName);
        user.AssignRole(role);
        return user;
    }

    #endregion

    #region Methods

    public Result AssignRole(Role role)
    {
        if (string.IsNullOrWhiteSpace(role.Name))
        {
            return Result.Failure(
                DomainErrors.User.InvalidRoleName);
        }
        
        if (!IsInRole(role))
            _roles.Add(role);
        
        return Result.Success();
    }

    public Result RemoveRole(Role role)
    {
        #region Validate role

        if (string.IsNullOrWhiteSpace(role.Name))
        {
            return Result.Failure(
                DomainErrors.User.InvalidRoleName);
        }

        if (!IsInRole(role))
        {
            return Result.Failure(
                DomainErrors.User.RoleNotAssigned(role.Id));
        }

        #endregion

        #region Remove role

        _roles.Remove(role);

        #endregion

        return Result.Success();
    }

    public bool IsInRole(Role role) => _roles.Contains(role);

    /// <summary>
    /// Updates the details of the user.
    /// </summary>
    /// <param name="firstName">The new first name of the user.</param>
    /// <param name="lastName">The new last name of the user.</param>
    public Result UpdateDetails(FirstName firstName, LastName lastName)
    {
        #region Update fields
        
        FirstName = firstName;
        LastName = lastName;
        
        #endregion

        #region Domain Events
        
        RaiseDomainEvent(new UserUpdatedDomainEvent(
            Guid.NewGuid(),
            Id,
            firstName.Value,
            lastName.Value));
        
        #endregion

        return Result.Success();
    }

    public Result ChangePassword(string newPasswordHash)
    {
        #region Validate password
        
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            return Result.Failure(
                DomainErrors.User.InvalidPasswordChange);
        }
        
        #endregion

        #region Update fields

        PasswordHash = newPasswordHash;

        #endregion

        return Result.Success();
    }

    #endregion
}
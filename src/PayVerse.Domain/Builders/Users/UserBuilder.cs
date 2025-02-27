using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.ValueObjects.Users;

namespace PayVerse.Domain.Builders.Users;

/// <summary>
/// Builder for creating User entities
/// </summary>
/// <remarks>
/// Constructor with required parameters
/// </remarks>
public class UserBuilder(string email,
                         string passwordHash,
                         string firstName,
                         string lastName) : IBuilder<User>
{
    #region Private Properties

    // Required parameters
    private readonly Email _email = Email.Create(email).Value;
    private readonly FirstName _firstName = FirstName.Create(firstName).Value;
    private readonly LastName _lastName = LastName.Create(lastName).Value;

    // Optional parameters
    private readonly List<Role> _roles = [];
    private bool _twoFactorEnabled;

    #endregion

    #region Building Blocks

    /// <summary>
    /// Adds a role to the user
    /// </summary>
    public UserBuilder WithRole(Role role)
    {
        if (!_roles.Contains(role))
        {
            _roles.Add(role);
        }

        return this;
    }

    /// <summary>
    /// Adds the business user role
    /// </summary>
    public UserBuilder AsBusinessUser()
    {
        return WithRole(Role.BusinessUser);
    }

    /// <summary>
    /// Adds the individual user role
    /// </summary>
    public UserBuilder AsIndividualUser()
    {
        return WithRole(Role.IndividualUser);
    }

    /// <summary>
    /// Adds the admin role
    /// </summary>
    public UserBuilder AsAdmin()
    {
        return WithRole(Role.Admin);
    }

    /// <summary>
    /// Enables two-factor authentication for the user
    /// </summary>
    public UserBuilder WithTwoFactorAuthentication()
    {
        _twoFactorEnabled = true;
        return this;
    }

    #endregion

    #region Build Construct

    /// <summary>
    /// Builds the User instance
    /// </summary>
    public User Build()
    {
        var user = User.Create(
            Guid.NewGuid(),
            _email,
            passwordHash,
            _firstName,
            _lastName,
            _roles.First()); // actually fix this - coming soon

        // Add roles
        foreach (var role in _roles)
        {
            user.AssignRole(role);
        }

        if (_twoFactorEnabled)
        {
            user.EnableTwoFactorAuthentication();
        }

        return user;
    }

    #endregion
}
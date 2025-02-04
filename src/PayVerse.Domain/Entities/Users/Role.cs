using PayVerse.Domain.Primitives;

namespace PayVerse.Domain.Entities.Users;

/// <summary> 
/// Represents a user role in the system. 
/// </summary>
public sealed class Role(
    int id,
    string name) : Enumeration<Role>(id, name)
{
    public static readonly Role Admin = new(1, "Admin");
    public static readonly Role BusinessUser = new(2, "BusinessUser");
    public static readonly Role IndividualUser = new(3, "IndividualUser");

    public ICollection<User> Users { get; set; }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayVerse.Domain.Entities.Users;
using PayVerse.Persistence.Users.Constants;

namespace PayVerse.Persistence.Users.Configurations;

/// <summary> 
/// Configures the Role entity for Entity Framework Core. 
/// </summary>
internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Map to the Roles table
        builder.ToTable(UserTableNames.Roles);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        builder.HasMany(x => x.Users)
            .WithMany(x => x.Roles);

        // Seed initial data
        builder.HasData(Role.GetValues());
    }
}

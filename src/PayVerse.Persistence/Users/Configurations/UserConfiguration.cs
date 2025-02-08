﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PayVerse.Domain.Entities;
using PayVerse.Domain.Entities.Users;
using PayVerse.Domain.ValueObjects;
using PayVerse.Domain.ValueObjects.Users;
using PayVerse.Persistence.Users.Constants;

namespace PayVerse.Persistence.Users.Configurations;

/// <summary> 
/// Configures the User entity for Entity Framework Core. 
/// </summary>
internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Map to the Users table
        builder.ToTable(UserTableNames.Users);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure property conversions and constraints
        builder
            .Property(x => x.Email)
            .HasConversion(x => x.Value, v => Email.Create(v).Value);
        
        builder
            .Property(x => x.FirstName)
            .HasConversion(x => x.Value, v => FirstName.Create(v).Value)
            .HasMaxLength(FirstName.MaxLength);
        
        builder
            .Property(x => x.LastName)
            .HasConversion(x => x.Value, v => LastName.Create(v).Value)
            .HasMaxLength(LastName.MaxLength);
        
        builder
            .Property(x => x.IsBlocked)
            .HasDefaultValue(false);
        
        builder
            .Property(x => x.TwoFactorEnabled)
            .HasDefaultValue(false);

        // Configure unique constraint on Email
        builder.HasIndex(x => x.Email).IsUnique();
    }
}
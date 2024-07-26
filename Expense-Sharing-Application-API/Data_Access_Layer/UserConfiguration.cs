using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Data_Access_Layer.Data
{
    [ExcludeFromCodeCoverage]
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
           

            builder.HasData(
                new User
                {
                    UserId = 1,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "admin@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "admin",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 2,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul1@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 3,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul2@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 4,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul3@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 5,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul4@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 6,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul5@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 7,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul6@gmail.com",
                    NormalizedEmail =null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 8,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul7@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                },
                new User
                {
                    UserId = 9,
                    Id = Guid.NewGuid().ToString(),
                    UserName = null,
                    NormalizedUserName = null,
                    Email = "rahul8@gmail.com",
                    NormalizedEmail = null,
                    EmailConfirmed = false,
                    PasswordHash = null,
                    SecurityStamp = string.Empty,
                    Role = "normal",
                    Password = "string",
                    Balance = 0
                }
            );
        }
    }
}

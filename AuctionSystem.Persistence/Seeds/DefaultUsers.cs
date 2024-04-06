using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Common;
using System;
using System.Collections.Generic;

namespace AuctionSystem.Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static List<User> UserList()
        {
            return new List<User>()
            {
                new User
                {
                    Id = RoleConstants.AdministratorUser,
                    UserName = "chiamaka",
                    Email = "juliusanwuli@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail= "JULIUSANWULI@GMAIL.COM",
                    NormalizedUserName="CHIAMAKA",
                    FirstName = "Chiamaka",
                    LastName = "Adegunju",
                    Role = UserRole.Administrator,
                    // Password = Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    IsActive = true,
                    IsLoggedIn = false
                },
                new User
                {
                    Id = RoleConstants.InitiatorUser,
                    UserName = "mastre",
                    Email = "adepeace200@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail= "ADEPEACE200@GMAIL.COM",
                    NormalizedUserName="MASTRE",
                    FirstName = "Peace",
                    LastName = "Adegunju",
                    Role = UserRole.Initiator,
                    // Password = Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    IsActive = true,
                    IsLoggedIn = false
                },
                new User
                {
                    Id = RoleConstants.AuthorizerUser,
                    UserName = "shine",
                    Email = "adorable@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail= "ADORABLE@GMAIL.COM",
                    NormalizedUserName="SHINE",
                    FirstName = "Lea",
                    LastName = "Mastre",
                    Role = UserRole.Authorizer,
                    // Password = Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    IsActive = true,
                    IsLoggedIn = false
                }
            };
        }
    }
}
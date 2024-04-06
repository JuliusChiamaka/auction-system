using AuctionSystem.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuctionSystem.Persistence.Seeds
{
    public static class ContextSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CreateBasicUsers(modelBuilder);
        }

        private static void CreateBasicUsers(ModelBuilder modelBuilder)
        {
            List<User> users = DefaultUsers.UserList();
            modelBuilder.Entity<User>().HasData(users);
        }
    }
}

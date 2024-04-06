using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;

namespace AuctionSystem.Domain.Auth
{
    public class User : IdentityUser, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
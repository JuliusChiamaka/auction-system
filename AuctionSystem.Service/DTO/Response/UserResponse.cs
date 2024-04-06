using AuctionSystem.Domain.Common;
using System;

namespace AuctionSystem.Service.DTO.Response
{
    public class UserResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public UserRole Role { get; set; }
        public string FormattedRole => Enum.GetName(typeof(UserRole), Role);
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }
}

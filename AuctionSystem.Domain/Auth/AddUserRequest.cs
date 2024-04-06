using AuctionSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Domain.Auth
{
    public class AddUserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //public string PhoneNumber { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }
    }
}

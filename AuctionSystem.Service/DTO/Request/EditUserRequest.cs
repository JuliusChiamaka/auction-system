using AuctionSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Service.DTO.Request
{
    public class EditUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserRole Role { get; set; }
    }
}

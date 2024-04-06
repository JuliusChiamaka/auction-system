using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Service.DTO.Request
{
    public class ResetUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Email { get; set; }
    }
}

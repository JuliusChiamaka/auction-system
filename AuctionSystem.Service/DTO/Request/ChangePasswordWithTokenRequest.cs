using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Service.DTO.Request
{
    public class ChangePasswordWithTokenRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

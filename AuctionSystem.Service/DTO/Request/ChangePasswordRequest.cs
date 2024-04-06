using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Service.DTO.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}

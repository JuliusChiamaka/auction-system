using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Service.DTO.Request
{
    public class DeleteUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }
    }
}

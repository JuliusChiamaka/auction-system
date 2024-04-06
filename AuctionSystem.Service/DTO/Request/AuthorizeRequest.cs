using AuctionSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Service.DTO.Request
{
    public class AuthorizeRequest
    {
        [DataType(DataType.Text)]
        [Required]
        public string Id { get; set; }
        [Required]
        [EnumDataType(typeof(AuthorizersStatus))]
        public AuthorizersStatus AuthStatus { get; set; }
        [DataType(DataType.Text)]
        [Required]
        public string AuthorizersComment { get; set; }
    }
}

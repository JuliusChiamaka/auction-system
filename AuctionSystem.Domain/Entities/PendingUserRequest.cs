using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Domain.Entities
{
    public class PendingUser : AuthorizableEntity
    {
        [Column("USER_NAME")]
        public string UserName { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        public string LastName { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("ROLE")]
        public UserRole Role { get; set; }
        [Column("REQUEST_TYPE")]
        public UserRequestType RequestType { get; set; }
    }
}

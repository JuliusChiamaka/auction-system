using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class Bid : AuthorizableEntity
    {
        public int BidId { get; set; }
        public string BiddingRoomId { get; set; } // Foreign key to BiddingRoom
        public int UserId { get; set; } // Foreign key to User
        public decimal BidAmount { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation properties
        public virtual BiddingRoom BiddingRoom { get; set; }
        public virtual User User { get; set; }
    }

}

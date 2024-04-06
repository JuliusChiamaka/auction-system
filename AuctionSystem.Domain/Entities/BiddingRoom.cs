using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class BiddingRoom : AuthorizableEntity
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public BiddingRoomStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal CurrentHighestBid { get; set; }
        public decimal InitialPrice { get; set; } // Add InitialPrice property
        public string ItemId { get; set; } // Foreign key to Item

        // Navigation property
        public virtual Item Item { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }


}

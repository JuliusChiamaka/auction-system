using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Request
{
    public class AddBiddingRoomRequest
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public BiddingRoomStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal CurrentHighestBid { get; set; }
        public string ItemId { get; set; }
        public Item Item { get; set; }
    }
}

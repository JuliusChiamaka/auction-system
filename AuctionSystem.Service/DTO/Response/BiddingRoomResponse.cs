using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Response
{
    public class BiddingRoomResponse
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public BiddingRoomStatus Status { get; set; }
        public string BiddingRoomStatusValue => Enum.GetName(typeof(BiddingRoomStatus), Status);
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ItemId { get; set; }
        public Item Item { get; set; }
        public string Initiator { get; set; }
        public DateTime DateInitiated { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Request
{
    public class PlaceBidRequest
    {
        public string RoomId { get; set; } 
        public int UserId { get; set; } 
        public decimal BidAmount { get; set; } 
    }

}

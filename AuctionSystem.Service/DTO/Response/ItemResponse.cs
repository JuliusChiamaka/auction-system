using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Response
{
    public class ItemResponse
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentHighestBid { get; set; }
    }
}

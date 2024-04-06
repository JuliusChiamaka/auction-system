using AuctionSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class Item : AuthorizableEntity
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentHighestBid { get; set; }
    }
}

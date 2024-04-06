using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class Invoice : AuthorizableEntity
    {
        public string InvoiceID { get; set; }
        public string UserID { get; set; } 
        public string RoomID { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; } 

    }


}

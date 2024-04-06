using AuctionSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Request
{
    public class AddInvoiceRequest
    {
        public string UserID { get; set; }
        public string RoomID { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
    }
}

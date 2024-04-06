using AuctionSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Response
{
    public class InvoiceResponse
    {
        public int InvoiceID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public decimal TotalAmount { get; set; }
        public InvoiceStatus InvoiceStatus { get; set; }
    }
}

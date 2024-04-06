using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Request
{
    public class NewPaymentRequest
    {
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public string InvoiceId { get; set; }

    }

}

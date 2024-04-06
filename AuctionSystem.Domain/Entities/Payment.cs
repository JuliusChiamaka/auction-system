using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Entities
{
    public class Payment : AuthorizableEntity
    {
        public string PaymentId { get; set; }
        public string InvoiceId { get; set; } 
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; } 

        public virtual Invoice Invoice { get; set; }
    }

}

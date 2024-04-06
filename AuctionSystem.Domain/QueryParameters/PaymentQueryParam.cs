using AuctionSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.QueryParameters
{
    public class PaymentQueryParam : UrlQueryParameters
    {
        public string Query { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Request
{
    public class VerificationRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Response
{
    public class EmailResponse
    {
        public string response_code { get; set; }
        public string response_message { get; set; }
        public string response_desc { get; set; }
        public EmailResponseData response_data { get; set; }
        public string responseTime { get; set; }
    }
    public class EmailResponseData
    {
        public string error { get; set; }
    }
}

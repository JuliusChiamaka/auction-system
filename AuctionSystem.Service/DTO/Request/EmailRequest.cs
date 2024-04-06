using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.DTO.Request
{
    public class EmailRequest
    {
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string MailMessage { get; set; }
        public List<IFormFile> Attachments { get; set; }

    }
}

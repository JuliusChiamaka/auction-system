using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Domain.Settings
{
    public class APISettingsOptions
    {
       
        public string MailBaseURL { get; set; }
        public string MailSenderEmail { get; set; }
        public string MailSender { get; set; }
        public string MailAuthKey { get; set; }
        
    }
}

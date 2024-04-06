using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract
{
    public interface INotificationService
    {
        Task<EmailResponse> SendEmail(EmailRequest request);
    }
}

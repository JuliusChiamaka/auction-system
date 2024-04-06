using AuctionSystem.Domain.Settings;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IClientFactory _clientFactory;
        private readonly APISettingsOptions _apiSettingsOptions;

        internal string _mailURL => _config["APISettings:MailBaseURL"];
        internal string _smsURL => _config["APISettings:SMSBaseURL"];
        internal string _authKey => _config["APISettings:AuthKey"];
        internal string _mailSenderEmail => _config["APISettings:MailSenderEmail"];
        internal string _mailSender => _config["APISettings:MailSender"];
        internal string _mailAuthKey => _config["APISettings:MailAuthKey"];
        public NotificationService(HttpClient httpClient,
            IConfiguration config,
            IClientFactory clientFactory, IOptions<APISettingsOptions> apiSettingsOptions)
        {
            _httpClient = httpClient;
            _config = config;
            _clientFactory = clientFactory;
            _apiSettingsOptions = apiSettingsOptions.Value;
        }

        public async Task<EmailResponse> SendEmail(EmailRequest request) 
        {
            request.From = _apiSettingsOptions.MailSenderEmail;
            request.DisplayName = _apiSettingsOptions.MailSender;
            var response = await _clientFactory.PostDataAsyncForm<EmailResponse, EmailRequest>(_apiSettingsOptions.MailBaseURL, request, _apiSettingsOptions.MailAuthKey);

            return response;
        }

        //public async Task SendPasswordResetToken(string userName, string resetUrl, string userFirstName, string userEmail)
        //{
        //    var emailRequest = new EmailRequest
        //    {
        //        from = _apiSettingsOptions.MailSenderEmail,
        //        displayName = _apiSettingsOptions.MailSender,
        //        to = userEmail, // Send the email to the user's email address
        //        subject = "Password Reset",
        //        mailMessage = $"Hi {userFirstName},\n\nYou can reset your password by clicking on the following link:\n{resetUrl}"
        //    };

        //    var response = await SendEmail(emailRequest);

        //    if (response.IsSuccess)
        //    {
        //        // Log or handle a successful email sending
        //    }
        //    else
        //    {
        //        // Handle the case where the email sending failed (e.g., log or throw an exception)
        //    }
        //}

        //public async Task<SMSResponse> SendSMS(SMSRequest dto)
        //{
        //    StringContent content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        //    //Add headers
        //    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _apiSettingsOptions.AuthKey);
        //    HttpResponseMessage httpResponse = await _httpClient.PostAsync(_apiSettingsOptions.SMSBaseURL, content);

        //    string jsonString = await httpResponse.Content.ReadAsStringAsync();
        //    SMSResponse data = JsonConvert.DeserializeObject<SMSResponse>(jsonString);

        //    return data;
        //}
    }
}

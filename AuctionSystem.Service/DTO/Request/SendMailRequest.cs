namespace AuctionSystem.Service.DTO.Request
{
    public class SendMailRequest
    {
        public string From { get; set; } = "no-reply@accessbankplc.com";
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string DisplayAsHtml { get; set; } = "true";
        public string DisplayName { get; set; } = "IMTO Portal Admin";
        public string CopyAddress { get; set; }
    }
}

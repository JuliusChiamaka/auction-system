using AuctionSystem.Domain.Common;

namespace AuctionSystem.Domain.QueryParameters
{
    public class PendingUserQueryParameters : UrlQueryParameters
    {
        public string Query { get; set; }
        public UserRole Role { get; set; }
        public UserRequestType RequestType { get; set; }
    }
}

using AuctionSystem.Domain.Common;

namespace AuctionSystem.Domain.QueryParameters
{
    public class UserQueryParameters : UrlQueryParameters
    {
        public string Query { get; set; }
        public string Role { get; set; }
        public int Status { get; set; }
    }
}

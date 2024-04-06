using AuctionSystem.Domain.Common;
using System;

namespace AuctionSystem.Service.DTO.Response
{
    public class PendingUserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public UserRequestType RequestType { get; set; }
        public string UserRequestTypeMeaning => Enum.GetName(typeof(UserRequestType), RequestType);
        public AuthStatus AuthStatus { get; set; }
        public string AuthStatusMeaning => Enum.GetName(typeof(AuthStatus), AuthStatus);





    }
}

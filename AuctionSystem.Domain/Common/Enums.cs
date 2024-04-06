
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Domain.Common
{

    public enum BiddingRoomStatus
    {
        Active,
        Inactive,
        Ended
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Completed = 1, 
    }
    public enum InvoiceStatus
    {
        Paid = 1,
        Pending = 2
    }

    public enum UserRole
    {
        Initiator = 1,
        Authorizer = 2,
        Administrator = 3
    }

    public enum UserRequestType
    {
        New = 1,
        Edit = 2,
        Delete = 3
    }

    public enum AuthStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Reinitiated = 4
    }

    public enum AuthorizersStatus
    {
        Approved = 1,
        Rejected = 2
    }

    public enum UserType
    {
        LandLord = 0,
        Agent = 1,
        Tenant = 2
    }

    
}
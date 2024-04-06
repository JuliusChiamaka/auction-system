using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Base;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Repository
{
    public interface IPendingUserRepository : IRepository<PendingUser>
    {
        Task<PagedList<PendingUser>> GetPendingUserListAsync(PendingUserQueryParameters queryParameters);
        Task<PendingUser> GetPendingUserByIdAsync(string UserId);
        Task<bool> IsUsernamePendingAsync(string username);

    }
}

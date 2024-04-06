using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Base;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<PagedList<User>> GetUserListAsync(UserQueryParameters queryParameters);
        Task<User> GetUserByIdAsync(string UserId);
    }
}

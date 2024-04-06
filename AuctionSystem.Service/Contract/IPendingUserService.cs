using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract
{ 
    public interface IPendingUserService
    {
        Task<PagedResponse<List<PendingUserResponse>>> GetPendingUsersAsync(PendingUserQueryParameters queryParameters);
        Task<Response<PendingUserResponse>> GetPendingUsersById(string id);
    }
}

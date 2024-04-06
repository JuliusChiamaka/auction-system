using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Service.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract
{
    public interface IBidService
    {
        Task<Response<string>> PlaceBidAsync(PlaceBidRequest request);
        Task<PagedResponse<Bid>> GetBidsByRoomId(int roomId, int pageNumber, int pageSize);
        Task<Response<Bid>> GetCurrentHighestBid(int roomId);
    }
}

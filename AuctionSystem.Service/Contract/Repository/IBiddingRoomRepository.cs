using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Repository
{
    public interface IBiddingRoomRepository : IRepository<BiddingRoom>
    {
        Task<PagedList<BiddingRoom>> GetBiddingRoomListAsync(BiddingRoomQueryParam queryParam);

        Task<BiddingRoom> GetBiddingRoomById(string RoomId);
        
    }
}

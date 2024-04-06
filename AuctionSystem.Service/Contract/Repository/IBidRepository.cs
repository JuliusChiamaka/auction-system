using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Service.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Repository
{
    public interface IBidRepository : IRepository<Bid>
    {
        Task<PagedList<Bid>> GetBidsByRoomId(string roomId, int pageNumber, int pageSize);
        Task<Bid> GetCurrentHighestBid(string roomId);
    }
}

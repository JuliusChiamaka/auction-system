using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Persistence;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Repository
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BidRepository(IServiceScopeFactory serviceScopeFactory, ApplicationDbContext dbContext)
            : base(serviceScopeFactory, (context) => context.Set<Bid>())
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Bid>> GetBidsByRoomId(string roomId, int pageNumber, int pageSize)
        {
            // Retrieve bids for the specified roomId
            var bids = await _dbContext.Bids
                .Where(b => b.BiddingRoomId == roomId)
                .OrderByDescending(b => b.BidAmount)
                .ToListAsync();

            return new PagedList<Bid>(bids, pageNumber, pageSize);
        }


        public async Task<Bid> GetCurrentHighestBid(string roomId)
        {
            return await _dbContext.Bids
                .Where(b => b.BiddingRoomId == roomId)
                .OrderByDescending(b => b.BidAmount)
                .FirstOrDefaultAsync();
        }
    }
}

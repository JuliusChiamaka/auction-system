using AuctionSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuctionSystem.Persistence
{
    public interface IApplicationDbContext
    {
        public DbSet<PendingUser> PendingUser { get; set; }
        public DbSet<BiddingRoom> BiddingRooms { get; set; }
        public DbSet<Bid> Bids { get; set; }


        Task<int> SaveChangesAsync();
        int SaveChanges();

    }
}
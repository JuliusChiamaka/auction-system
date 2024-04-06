using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.Entities.Base;
using AuctionSystem.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionSystem.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<PendingUser> PendingUser { get; set; }
        public DbSet<BiddingRoom> BiddingRooms { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            

            modelBuilder.Entity<PendingUser>(entity =>
            {
                entity.ToTable(name: "PENDING_USER");
                entity.Property(x => x.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "USER");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "ROLE");
            }).Model.SetMaxIdentifierLength(30);
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("USERROLES");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("USERCLAIMS");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("ROLECLAIMS");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("USERLOGINS");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("USERTOKENS");
            }).Model.SetMaxIdentifierLength(30);

            modelBuilder.Seed();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.Now; // current datetime

                if (entity.State == EntityState.Added)
                {
                    ((EntityBase)entity.Entity).CreatedAt = now;
                }
                ((EntityBase)entity.Entity).UpdatedAt = now;
            }
        }
    }
}
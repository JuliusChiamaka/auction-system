using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Persistence;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.Repository.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Repository
{
    public class BiddingRoomRepository : Repository<BiddingRoom>, IBiddingRoomRepository
    {
        public BiddingRoomRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<BiddingRoom>())
        { 
        }

        public async Task<BiddingRoom> GetBiddingRoomById(string RoomId)
        {
            return await GetFirstOrDefaultAsync(x => x.Id == RoomId.ToLower());
        }

        public async Task<PagedList<BiddingRoom>> GetBiddingRoomListAsync(BiddingRoomQueryParam queryParam)
        {
            var whereList = new List<Expression<Func<BiddingRoom, bool>>>();
            var orderByList = new List<Tuple<SortingOption, Expression<Func<BiddingRoom, object>>>>();

            if (!string.IsNullOrEmpty(queryParam.Query))
            {
                whereList.Add(p => p.RoomName.Contains(queryParam.Query)
                || p.RoomId.Contains(queryParam.Query)
                || p.ItemId.Contains(queryParam.Query));
            }

            orderByList.Add(new Tuple<SortingOption, Expression<Func<BiddingRoom, object>>>(new SortingOption { Direction = SortingOption.SortingDirection.DESC }, p => p.CreatedAt));

            PagedList<BiddingRoom> response = await GetPagedListAsync(queryParam, whereList, orderByList);

            return response;
        }
    }
}

using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
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
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<Item>())
        {
        }

        public async Task<Item> GetItemById(string ItemId)
        {
            return await GetFirstOrDefaultAsync(x => x.ItemId == ItemId);
        }

        public async Task<PagedList<Item>> GetItemListAsync(ItemQueryParam queryParam)
        {
            var whereList = new List<Expression<Func<Item, bool>>>();
            var orderByList = new List<Tuple<SortingOption, Expression<Func<Item, object>>>>();

            if (!string.IsNullOrEmpty(queryParam.Query))
            {
                whereList.Add(p => p.ItemId.Contains(queryParam.Query)
                || p.ItemName.Contains(queryParam.Query)
                || p.Description.Contains(queryParam.Query));
            }

            orderByList.Add(new Tuple<SortingOption, Expression<Func<Item, object>>>(new SortingOption { Direction = SortingOption.SortingDirection.DESC }, p => p.CreatedAt));

            PagedList<Item> response = await GetPagedListAsync(queryParam, whereList, orderByList);

            return response;
        }
    }
}

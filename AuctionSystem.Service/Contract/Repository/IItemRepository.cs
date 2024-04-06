using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Base;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Repository
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<PagedList<Item>> GetItemListAsync(ItemQueryParam queryParam);
        Task<Item> GetItemById(string ItemId);
    }
}

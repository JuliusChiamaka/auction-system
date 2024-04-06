using System.Collections.Generic;

namespace AuctionSystem.Domain.Common
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public PagedList(List<T> items, int totalRecords, int totalPages)
        {
            Items = items;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
        }
    }
}

namespace AuctionSystem.Domain.Common
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public PagedResponse(T data, int pageNumber, int pageSize, int totalRecords, int totalPages, string message)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
            this.TotalPages = totalPages;
            this.Succeeded = true;
            this.Code = 200;
            this.Message = message;
            this.Data = data;
            this.Errors = null;
        }
    }
}

using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Base;
using AuctionSystem.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Repository
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<PagedList<Invoice>> GetAllInvoiceAsync(InvoiceQueryParam queryParam);
        Task<Invoice> GetInvoiceById(string invoiceId);
        Task<Invoice> MarkInvoiceAsPaid(string invoiceId);
    }
}

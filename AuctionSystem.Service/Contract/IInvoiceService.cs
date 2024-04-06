using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract
{
    public interface IInvoiceService
    {
        Task<Response<InvoiceResponse>> GenerateInvoice(AddInvoiceRequest request);
        Task<PagedResponse<List<Response<InvoiceResponse>>>> GetAllInvoiceAsync(InvoiceQueryParam queryParam);
        Task<Response<InvoiceResponse>> GetInvoiceById(string invoiceId);
        Task<Response<InvoiceResponse>> MarkInvoiceAsPaid(string invoiceId);
    }
}

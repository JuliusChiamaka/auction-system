using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.Exceptions;
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
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<Invoice>())
        {
        }

        public async Task<PagedList<Invoice>> GetAllInvoiceAsync(InvoiceQueryParam queryParam)
        {
            var whereList = new List<Expression<Func<Invoice, bool>>>();
            var orderByList = new List<Tuple<SortingOption, Expression<Func<Invoice, object>>>>();

            if (!string.IsNullOrEmpty(queryParam.Query))
            {
                whereList.Add(p => p.InvoiceID.Contains(queryParam.Query)
                || p.RoomID.Contains(queryParam.Query)
                || p.InvoiceStatus.Equals(queryParam.Query));
            }

            orderByList.Add(new Tuple<SortingOption, Expression<Func<Invoice, object>>>(new SortingOption { Direction = SortingOption.SortingDirection.DESC }, p => p.CreatedAt));

            PagedList<Invoice> response = await GetPagedListAsync(queryParam, whereList, orderByList);

            return response;
        }

        public async Task<Invoice> GetInvoiceById(string invoiceId)
        {
            return await GetFirstOrDefaultAsync(x => x.InvoiceID == invoiceId);
        }

        public async Task<Invoice> MarkInvoiceAsPaid(string invoiceId)
        {
            var invoice = await GetInvoiceById(invoiceId);
            if (invoice == null)
            {
                throw new ApiException($"Invoice with ID '{invoiceId}' not found.");
            }

            // Update the invoice status to "paid"
            invoice.InvoiceStatus = InvoiceStatus.Paid;
            invoice.TimeStamp = DateTime.Now;

            // Update the invoice in the database
            await UpdateAsync(invoice);

            return invoice;
        }


    }
}

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
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(IServiceScopeFactory serviceScopeFactory)
           : base(serviceScopeFactory, (context) => context.Set<Payment>())
        {
        }

        public async Task<PagedList<Payment>> ProcessedPaymentPagedList(PaymentQueryParam param)
        {
            var whereList = new List<Expression<Func<Payment, bool>>>();
            var orderByList = new List<Tuple<SortingOption, Expression<Func<Payment, object>>>>();

            if (!string.IsNullOrEmpty(param.Query))
            {
                whereList.Add(p => p.PaymentId.Contains(param.Query)
                || p.PaymentId.Contains(param.Query)
                || p.Status.Equals(param.Query));
            }

            orderByList.Add(new Tuple<SortingOption, Expression<Func<Payment, object>>>(new SortingOption { Direction = SortingOption.SortingDirection.DESC }, p => p.CreatedAt));

            PagedList<Payment> response = await GetPagedListAsync(param, whereList, orderByList);

            return response;
        }

        public async Task<Payment> VerifyPayment(string paymentId)
        {
            return await GetFirstOrDefaultAsync(x => x.PaymentId == paymentId);
        }
    }
}

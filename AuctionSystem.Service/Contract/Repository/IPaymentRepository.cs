using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Repository
{
    public interface IPaymentRepository :IRepository<Payment>
    {
        Task<Payment> VerifyPayment(string paymentId);
        Task<PagedList<Payment>> ProcessedPaymentPagedList(PaymentQueryParam param);
    }
}

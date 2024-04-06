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
    public interface IPaymentService
    {
        Task<Response<PaymentResponse>> ProcessPayment(NewPaymentRequest request);
        Task<Response<PaymentResponse>> VerifyPayment(string paymentId);
        Task<PagedResponse<List<PaymentResponse>>> GetProcessedPayment(PaymentQueryParam param);

    }
}

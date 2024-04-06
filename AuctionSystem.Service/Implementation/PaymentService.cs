using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using AuctionSystem.Service.Exceptions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IInvoiceRepository invoiceRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _mapper = mapper;
        }

        public async Task<Response<PaymentResponse>> ProcessPayment(NewPaymentRequest request)
        {
            // Validate request data
            if (request == null)
            {
                throw new ApiException("Invalid request data.");
            }

            // Retrieve the invoice by ID
            var invoice = await _invoiceRepository.GetInvoiceById(request.InvoiceId);
            if (invoice == null)
            {
                throw new ApiException($"Invoice with ID '{request.InvoiceId}' not found.");
            }

            // Check if the provided user ID matches the user associated with the invoice
            if (invoice.UserID != request.UserId)
            {
                throw new ApiException("Unauthorized access to invoice payment.");
            }

            // Create a new payment entity
            var payment = new Payment
            {
                InvoiceId = request.InvoiceId,
                Amount = request.Amount,
                // Set other payment properties as needed
            };

            // Save the payment to the database
            var createdPayment = await _paymentRepository.AddAsync(payment);

            // Map the created payment to a response object
            var paymentResponse = _mapper.Map<PaymentResponse>(createdPayment);

            // Return the response
            return new Response<PaymentResponse>(paymentResponse, message: "Payment processed successfully.");
        }
        public async Task<Response<PaymentResponse>> VerifyPayment(string paymentId)
        {
            // Retrieve payment from the repository
            var payment = await _paymentRepository.VerifyPayment(paymentId);

            if (payment == null)
            {
                throw new ApiException($"Payment with ID '{paymentId}' not found.");
            }

            // Map payment to response object
            var paymentResponse = _mapper.Map<PaymentResponse>(payment);

            // Return the response
            return new Response<PaymentResponse>(paymentResponse, message: "Payment verified successfully.");
        }

        public async Task<PagedResponse<List<PaymentResponse>>> GetProcessedPayment(PaymentQueryParam param)
        {
            // Retrieve processed payments from the repository
            var processedPayments = await _paymentRepository.ProcessedPaymentPagedList(param);

            // Map payments to response objects
            List<PaymentResponse> response = _mapper.Map<List<Payment>, List<PaymentResponse>>(processedPayments.Items);

            // Return paged response
            return new PagedResponse<List<PaymentResponse>>(response, param.PageNumber, param.PageSize, processedPayments.TotalRecords, processedPayments.TotalPages, $"Processed payments retrieved successfully.");
        }
    }

}

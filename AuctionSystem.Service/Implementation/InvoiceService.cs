using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using AuctionSystem.Service.Exceptions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Implementation
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IBiddingRoomRepository _biddingRoomRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<Invoice> _logger;

        public InvoiceService(IInvoiceRepository invoiceRepository,
            IBiddingRoomRepository biddingRoomRepository,
            IBidRepository bidRepository,
            IMapper mapper,
            ILogger<Invoice> logger)
        {
            _invoiceRepository = invoiceRepository;
            _biddingRoomRepository = biddingRoomRepository;
            _bidRepository = bidRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<InvoiceResponse>> GenerateInvoice(AddInvoiceRequest request)
        {
            // Retrieve the highest bid for the specified room
            var highestBid = await _bidRepository.GetCurrentHighestBid(request.RoomID);
            if (highestBid == null)
            {
                throw new ApiException("No bids found for the specified room.");
            }

            // Retrieve the bidding room details
            var biddingRoom = await _biddingRoomRepository.GetBiddingRoomById(request.RoomID);
            if (biddingRoom == null)
            {
                throw new ApiException($"Bidding room with ID '{request.RoomID}' not found.");
            }

            // Calculate the total amount to be paid based on the highest bid
            decimal totalAmount = highestBid.BidAmount;

            // Create a new invoice
            var invoice = new Invoice
            {
                RoomID = request.RoomID,
                TotalAmount = totalAmount,
                InvoiceStatus = InvoiceStatus.Pending, 
                CreatedAt = DateTime.Now
            };

            await _invoiceRepository.AddAsync(invoice);

            var response = _mapper.Map<InvoiceResponse>(invoice);

            return new Response<InvoiceResponse>(response, message: "Invoice generated successfully.");
        }

        public Task<PagedResponse<List<Response<InvoiceResponse>>>> GetAllInvoiceAsync(InvoiceQueryParam queryParam)
        {
            throw new NotImplementedException();
        }

        public Task<Response<InvoiceResponse>> GetInvoiceById(string invoiceId)
        {
            throw new NotImplementedException();
        }

        public Task<Response<InvoiceResponse>> MarkInvoiceAsPaid(string invoiceId)
        {
            throw new NotImplementedException();
        }
    }
}

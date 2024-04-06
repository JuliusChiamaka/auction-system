using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Implementation
{
    public class BidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IBiddingRoomRepository _biddingRoomRepository;

        public BidService(IBidRepository bidRepository, IBiddingRoomRepository biddingRoomRepository)
        {
            _bidRepository = bidRepository;
            _biddingRoomRepository = biddingRoomRepository;
        }

        public async Task<Response<string>> PlaceBidAsync(PlaceBidRequest request)
        {
            var biddingRoom = await _biddingRoomRepository.GetByIdAsync(request.RoomId);
            if (biddingRoom == null)
            {
                return new Response<string>(null, "Bidding room not found.");
            }

            
            if (request.BidAmount < biddingRoom.InitialPrice)
            {
                return new Response<string>(null, "Bid amount cannot be below the Initial Price of the bidding room.");
            }

            var currentHighestBid = await _bidRepository.GetCurrentHighestBid(request.RoomId);
            if (currentHighestBid != null && request.BidAmount <= currentHighestBid.BidAmount)
            {
                return new Response<string>(null, "Bid amount must be higher than the current highest bid.");
            }

            // Create a new Bid object
            var bid = new Bid
            {
                BiddingRoomId = request.RoomId,
                UserId = request.UserId,
                BidAmount = request.BidAmount,
                Timestamp = DateTime.Now
            };

            // Add the bid to the repository
            await _bidRepository.AddAsync(bid);

            return new Response<string>(bid.BidId.ToString(), "Bid placed successfully.");
        }



        public async Task<Response<Bid>> GetCurrentHighestBidAsync(string roomId)
        {
            // Retrieve the current highest bid for the specified room
            var highestBid = await _bidRepository.GetCurrentHighestBid(roomId);

            if (highestBid == null)
            {
                return new Response<Bid>(null, "No bids found for the specified room.");
            }

            return new Response<Bid>(highestBid, "Current highest bid retrieved successfully.");
        }

        public async Task<PagedResponse<List<Bid>>> GetBidsByRoomIdAsync(string roomId, int pageNumber, int pageSize)
        {
            // Retrieve bids for the specified room
            var bids = await _bidRepository.GetBidsByRoomId(roomId, pageNumber, pageSize);

            return new PagedResponse<List<Bid>>(bids.Items, pageNumber, pageSize, bids.TotalRecords, bids.TotalPages, "Bids retrieved successfully.");
        }
    }

}

using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using AuctionSystem.Service.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Implementation
{
    
    public class BiddingRoomService : IBiddingRoomService
    {
        private readonly IBiddingRoomRepository _biddingRoomRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger<BiddingRoomService> _logger;
        public BiddingRoomService(IBiddingRoomRepository biddingRoomRepository,
            IItemRepository itemRepository,
            IMapper mapper,
            ILogger<BiddingRoomService> logger,
            IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = logger;
            _biddingRoomRepository = biddingRoomRepository;
            _itemRepository = itemRepository;
        }
        public async Task<Response<BiddingRoomResponse>> CreateBiddingRoomAsync(AddBiddingRoomRequest request)
        {
            var existingBiddingRoom = await _biddingRoomRepository.GetAsync(b => b.RoomName == request.RoomName);

            if (existingBiddingRoom.Any())
            {
                throw new ApiException($"Room with name '{request.RoomName}' has already been registered.");
            }

            if (request.StartTime >= request.EndTime)
            {
                throw new ApiException("Start time must be before end time.");
            }

            // Ensure that the initiator's username is retrieved correctly from the HTTP context
            string initiator = _httpContextAccessor.HttpContext.User.Claims
                                    .FirstOrDefault(x => x.Type == "username")?.Value;

            // Create the item along with the bidding room
            Item newItem = new Item
            {
                ItemName = request.Item.ItemName,
                Description = request.Item.Description,
                InitialPrice = request.Item.InitialPrice,
                CurrentHighestBid = 0 // Initial highest bid is 0
            };

            // Add the item to the database
            await _itemRepository.AddAsync(newItem);

            // Create the bidding room with an empty collection of bids
            BiddingRoom room = new BiddingRoom
            {
                RoomId = request.RoomId,
                RoomName = request.RoomName,
                Status = BiddingRoomStatus.Inactive,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                CurrentHighestBid = 0, // Initial highest bid is 0
                ItemId = newItem.ItemId, // Use the ID of the newly created item
                Initiator = initiator,
                DateInitiated = DateTime.Now,
                Bids = new List<Bid>() // Initialize an empty collection of bids
            };

            // Add the bidding room to the database
            await _biddingRoomRepository.AddAsync(room);

            // Map the created bidding room to BiddingRoomResponse using AutoMapper
            BiddingRoomResponse response = _mapper.Map<BiddingRoomResponse>(room);

            return new Response<BiddingRoomResponse>(response, message: $"Successfully created a bidding room with id - {request.RoomId}");
        }
                               
        public async Task<Response<BiddingRoomResponse>> GetBiddingRoomById(string roomId)
        {
            BiddingRoom room = await _biddingRoomRepository.GetByIdAsync(roomId);

            if (room == null)
            {
                throw new ApiException($"No Bidding Room found for Room ID - {roomId}");
            }

            BiddingRoomResponse response = _mapper.Map<BiddingRoom, BiddingRoomResponse>(room);
            return new Response<BiddingRoomResponse>(response, $"Successfully retrieved Room detailss for Bidding Room with ID - {roomId}");
        }

        public async Task<PagedResponse<List<BiddingRoomResponse>>> GetBiddingRooms(BiddingRoomQueryParam queryParam)
        {
            var pagedList = await _biddingRoomRepository.GetBiddingRoomListAsync(queryParam);
            List<BiddingRoomResponse> response = _mapper.Map<List<BiddingRoom>, List<BiddingRoomResponse>>(pagedList.Items);

            return new PagedResponse<List<BiddingRoomResponse>>(response, queryParam.PageNumber, queryParam.PageSize, pagedList.TotalRecords, pagedList.TotalPages, $"Successfully retrieved Bidding Rooms");
        }

        public async Task<Response<BiddingRoomResponse>> StartBidding(string roomId)
        {
            // Retrieve the bidding room from the repository
            var biddingRoom = await _biddingRoomRepository.GetByIdAsync(roomId);

            // Check if the bidding room exists and if it's inactive
            if (biddingRoom == null || biddingRoom.Status != BiddingRoomStatus.Inactive)
            {
                return new Response<BiddingRoomResponse>(null, $"Bidding room with id {roomId} either not found or not in the correct state to start bidding.");
            }

            // Update the bidding room status to Active
            biddingRoom.Status = BiddingRoomStatus.Active;

            // Initialize the start time
            biddingRoom.StartTime = DateTime.Now;

            // Set the end time to 20 minutes from the start time
            biddingRoom.EndTime = biddingRoom.StartTime.AddMinutes(20);

            // Save changes to the database
            await _biddingRoomRepository.UpdateAsync(biddingRoom);

            // Start a background task to monitor the bidding process and end the bidding room if necessary
            _ = MonitorBiddingProcess(roomId);

            // Map the updated bidding room to BiddingRoomResponse
            var biddingRoomResponse = _mapper.Map<BiddingRoomResponse>(biddingRoom);

            return new Response<BiddingRoomResponse>(biddingRoomResponse, $"Bidding room with id {roomId} started successfully.");
        }

        public async Task<Response<BiddingRoomResponse>> EndBidding(string roomId)
        {
            // Retrieve the bidding room from the repository
            var biddingRoom = await _biddingRoomRepository.GetByIdAsync(roomId);

            // Check if the bidding room exists and if it's active
            if (biddingRoom == null || biddingRoom.Status != BiddingRoomStatus.Active)
            {
                return new Response<BiddingRoomResponse>(null, $"Bidding room with id {roomId} either not found or not in the correct state to end bidding.");
            }

            // Update the bidding room status to Inactive
            biddingRoom.Status = BiddingRoomStatus.Inactive;

            // Save changes to the database
            await _biddingRoomRepository.UpdateAsync(biddingRoom);

            // Map the updated bidding room to BiddingRoomResponse
            var biddingRoomResponse = _mapper.Map<BiddingRoomResponse>(biddingRoom);

            return new Response<BiddingRoomResponse>(biddingRoomResponse, $"Bidding room with id {roomId} ended successfully.");
        }

        private async Task MonitorBiddingProcess(string roomId)
        {
            while (true)
            {
                // Retrieve the bidding room from the repository
                var biddingRoom = await _biddingRoomRepository.GetByIdAsync(roomId);

                // Check if the bidding room exists and if the current time exceeds the end time
                if (biddingRoom == null || DateTime.Now >= biddingRoom.EndTime)
                {
                    // End the bidding room
                    await EndBidding(roomId);
                    break;
                }

                // Sleep for a short interval before checking again
                await Task.Delay(5000); // Check every 5 seconds
            }
        }

    }
}

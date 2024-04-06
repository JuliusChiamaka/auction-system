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
    public interface IBiddingRoomService
    {
        Task<Response<BiddingRoomResponse>> CreateBiddingRoomAsync(AddBiddingRoomRequest request);
        Task<Response<BiddingRoomResponse>> StartBidding(string roomId);
        Task<Response<BiddingRoomResponse>> EndBidding(string roomId);
        Task<PagedResponse<List<BiddingRoomResponse>>> GetBiddingRooms(BiddingRoomQueryParam queryParam);
        Task<Response<BiddingRoomResponse>> GetBiddingRoomById(string roomId);
    }
}

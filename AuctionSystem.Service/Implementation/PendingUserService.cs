using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.DTO.Response;
using AuctionSystem.Service.Exceptions;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Implementation
{
    public class PendingUserService : IPendingUserService
    {
        private readonly IPendingUserRepository _pendingUserRepository;
        private readonly IMapper _mapper;

        public PendingUserService(IPendingUserRepository pendingUserRepository, IMapper mapper)
        {

            _pendingUserRepository = pendingUserRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<PendingUserResponse>>> GetPendingUsersAsync(PendingUserQueryParameters queryParameters)
        {
            var pagedList = await _pendingUserRepository.GetPendingUserListAsync(queryParameters);

            if (pagedList.Items.Count <= 0)
            {
                // static response indicating no users were found.
                var emptyResponse = new PagedResponse<List<PendingUserResponse>>(
                    new List<PendingUserResponse>(),
                    queryParameters.PageNumber,
                    queryParameters.PageSize,
                    0,
                    0,
                    "There are no pending users awaiting approval ."
                );
                return emptyResponse;
            }

            List<PendingUserResponse> response = _mapper.Map<List<PendingUser>, List<PendingUserResponse>>(pagedList.Items);

            return new PagedResponse<List<PendingUserResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, pagedList.TotalRecords, pagedList.TotalPages, "Successfully retrieved users");
        }


        public async Task<Response<PendingUserResponse>> GetPendingUsersById(string id)
        {
            var pendingUser = await _pendingUserRepository.GetPendingUserByIdAsync(id);

            if (pendingUser == null)
            {
                throw new ApiException($"No user found for User ID - {id}.");
            }

            PendingUserResponse response = _mapper.Map<PendingUser, PendingUserResponse>(pendingUser);

            return new Response<PendingUserResponse>(response, $"Successfully retrieved user details for user with Id - {id}");
        }
    }
}

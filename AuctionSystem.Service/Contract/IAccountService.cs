using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract
{
    public interface IAccountService
    {
        Task<Response<string>> AddUserAsync(AddUserRequest request);
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<Response<string>> LogoutAsync();
        Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters);
        Task<Response<UserResponse>> GetUserById(string id);
        Task<Response<string>> ValidateUsernameAsync(string username);
        Task<Response<string>> EditUserAsync(EditUserRequest request);
        Task<Response<string>> DeleteUserAsync(DeleteUserRequest request);
        Task<Response<string>> ResetUserAsync(ResetUserRequest request);
        Task<Response<string>> AuthorizeUserAsync(AuthorizeRequest request);
        Task<Response<string>> ChangePasswordWithToken(ChangePasswordWithTokenRequest request);
        Task<Response<string>> ChangePassword(ChangePasswordRequest request);
        Task<Response<string>> PasswordReset(PasswordResetRequest request);
    }
}

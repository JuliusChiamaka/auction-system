using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.DTO.Request; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuctionSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IPendingUserService _pendingUserService;
        private readonly INotificationService _notificationService;
        public AccountController(IAccountService accountService, IPendingUserService pendingUserService, INotificationService notificationService)
        {
            _accountService = accountService;
            _pendingUserService = pendingUserService;
            _notificationService = notificationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok(await _accountService.LogoutAsync());
        }
        [HttpGet("getPendingUsers")]
        public async Task<IActionResult> GetPendingUsers([FromQuery] PendingUserQueryParameters queryParameters)
        {
            return Ok(await _pendingUserService.GetPendingUsersAsync(queryParameters));
        }
        [HttpGet("getPendingUserById")]
        public async Task<IActionResult> GetPendingUserById(string id)
        {
            return Ok(await _pendingUserService.GetPendingUsersById(id));
        }
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryParameters queryParameters)
        {
            return Ok(await _accountService.GetUsersAsync(queryParameters));
        }
        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            return Ok(await _accountService.GetUserById(id));
        }
        [HttpPost("addUser")]
        public async Task<IActionResult> AddUser(AddUserRequest request)
        {
            return Ok(await _accountService.AddUserAsync(request));
        }
        [HttpGet("validateUser")]
        public async Task<IActionResult> ValidateUser([FromQuery] string userId)
        {
            return Ok(await _accountService.ValidateUsernameAsync(userId));
        }
        [HttpPost("editUser")]
        public async Task<IActionResult> EditUser([FromBody] EditUserRequest request)
        {
            return Ok(await _accountService.EditUserAsync(request));
        }
        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request)
        {
            return Ok(await _accountService.DeleteUserAsync(request));
        }
        [HttpPost("resetUser")]
        public async Task<IActionResult> ResetUser([FromBody] ResetUserRequest request)
        {
            return Ok(await _accountService.ResetUserAsync(request));
        }
        [HttpPost("authorizeUser")]
        public async Task<IActionResult> AuthorizeUser([FromBody] AuthorizeRequest request)
        {
            return Ok(await _accountService.AuthorizeUserAsync(request));
        }
        [HttpPost("changePasswordWithToken")]
        public async Task<IActionResult> ChangePasswordWithToken([FromBody] ChangePasswordWithTokenRequest request)
        {
            return Ok(await _accountService.ChangePasswordWithToken(request));
        }
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return Ok(await _accountService.ChangePassword(request));
        }
        [AllowAnonymous]
        [HttpPost("passwordReset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest request)
        {
            return Ok(await _accountService.PasswordReset(request));
        }
    }
}
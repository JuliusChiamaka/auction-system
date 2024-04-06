using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Domain.Settings;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using AuctionSystem.Service.Exceptions;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AuctionSystem.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly ApiResourceUrls _apiResourceUrls;
        private readonly ApplicationConfig _applicationConfig;
        private readonly IUserRepository _userRepository;
        private readonly IPendingUserRepository _pendingUserRepository;
        private readonly INotificationService _notificationService;
        private readonly ITemplateService _templateService;

        public AccountService(IOptions<JWTSettings> jwtSettings,
      UserManager<User> userManager,
      SignInManager<User> signInManager,
      IHttpContextAccessor httpContextAccessor,
      IMapper mapper,
      ILogger<AccountService> logger,
      IOptions<ApiResourceUrls> apiResourceUrl,
      IOptions<ApplicationConfig> applicationConfig,
      IUserRepository userRepository,
      IPendingUserRepository pendingUserRepository,
      ITemplateService templateService)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = logger;
            _userRepository = userRepository;
            _pendingUserRepository = pendingUserRepository;
            _apiResourceUrls = apiResourceUrl.Value;
            _applicationConfig = applicationConfig.Value;
            _templateService = templateService;
        }

        public async Task<Response<string>> AddUserAsync(AddUserRequest request)
        {
            request.UserName = request.UserName?.Trim();
            request.FirstName = request.FirstName?.Trim();
            request.LastName = request.LastName?.Trim();
            request.Email = request.Email?.Trim();


            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            var pendingUserWithUsername = await _pendingUserRepository.GetAsync(x => x.UserName == request.UserName && x.AuthStatus == AuthStatus.Pending);
            if (userWithSameUserName != null || userWithSameEmail != null || pendingUserWithUsername.Any())
            {
                throw new ApiException($"This user name or email is already registered or awaiting approval.");
            }

            string username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimFields.USER_NAME)?.Value;
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimFields.EMAIL)?.Value;

            PendingUser pendingUser = _mapper.Map<PendingUser>(request);

            pendingUser.RequestType = UserRequestType.New;
            pendingUser.AuthStatus = AuthStatus.Pending;
            pendingUser.Role = request.Role;
            pendingUser.Initiator = username;
            pendingUser.InitiatorEmail = email;
            pendingUser.DateInitiated = DateTime.Now;

            await _pendingUserRepository.AddAsync(pendingUser);

            return new Response<string>(pendingUser.Id, message: $"Successfully registered user with username - {request.UserName}");
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            // Check for the username
            User user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                throw new ApiException($"Invalid username or password.");
            }

            // Verify the username and password
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!signInResult.Succeeded)
            {
                throw new ApiException($"Invalid username or password.");
            }

            if (!user.IsActive)
            {
                throw new ApiException($"This account has not been authorized for use.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = _mapper.Map<User, AuthenticationResponse>(user);

            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.ExpiresIn = _jwtSettings.DurationInMinutes * 60;
            response.ExpiryDate = DateTime.Now.AddSeconds((_jwtSettings.DurationInMinutes * 60));

            user.IsLoggedIn = true;
            user.LastLoginTime = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roleClaims = new List<Claim>
            {
                new Claim("roles", user.Role.ToString())
            };

            DateTime utcNow = DateTime.UtcNow;
            string ipAddress = IpHelper.GetIpAddress();
            string sessionKey = Guid.NewGuid().ToString();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(UserClaimFields.CREATED_AT, utcNow.ToString()),
                new Claim(UserClaimFields.EMAIL, user.Email),
                new Claim(UserClaimFields.SESSION_ID, sessionKey),
                new Claim(UserClaimFields.USER_NAME, user.UserName),
                new Claim(UserClaimFields.FIRST_NAME, user.FirstName),
                new Claim(UserClaimFields.LAST_NAME, user.LastName),
                new Claim(UserClaimFields.IP_ADDRESS, ipAddress)//REMOVE IF IT DOESN'T EMAIL DOESN'T SHOW
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<Response<string>> LogoutAsync()
        {
            var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimFields.USER_NAME)?.Value;
            var sessionId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimFields.SESSION_ID)?.Value;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(sessionId))
            {
                return new Response<string>($"Invalid session", (int)HttpStatusCode.OK, true);
            }

            User user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return new Response<string>($"Invalid session", (int)HttpStatusCode.OK, true);
            }

            user.IsLoggedIn = false;
            await _userManager.UpdateAsync(user);

            return new Response<string>($"Successfully logged out user with username - {username}", (int)HttpStatusCode.OK, true);
        }

        public async Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters)
        {
            var pagedList = await _userRepository.GetUserListAsync(queryParameters);

            if (pagedList.Items.Count <= 0)
            {
                throw new ApiException($"No user found within the query parameters.");
            }

            List<UserResponse> response = _mapper.Map<List<User>, List<UserResponse>>(pagedList.Items);

            return new PagedResponse<List<UserResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, pagedList.TotalRecords, pagedList.TotalPages, $"Successfully retrieved users");
        }

        public async Task<Response<UserResponse>> GetUserById(string id)
        {
            var userData = await _userRepository.GetUserByIdAsync(id);

            if (userData == null)
            {
                throw new ApiException($"No user found for User ID - {id}.");
            }

            UserResponse response = _mapper.Map<User, UserResponse>(userData);

            return new Response<UserResponse>(response, $"Successfully retrieved user details for user with Id - {id}");
        }

        public async Task<Response<string>> ValidateUsernameAsync(string username)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(username);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{username}' is already registered.");
            }

            return new Response<string>("Success", "The username is valid");
        }

        public async Task<Response<string>> EditUserAsync(EditUserRequest request)
        {
            var isUsernamePending = await _pendingUserRepository.IsUsernamePendingAsync(request.UserName);

            if (isUsernamePending)
            {
                return new Response<string>(null, message: $"The operation for the username '{request.UserName}' is awaiting approval.");
            }


            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"Username '{request.UserName}' could not be found.");
            }

            string username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value;

            PendingUser pendingUserRequest = new PendingUser()
            {
                UserName = request.UserName,
                FirstName = string.IsNullOrEmpty(request.FirstName) ? user.FirstName : request.FirstName,
                LastName = string.IsNullOrEmpty(request.LastName) ? user.LastName : request.LastName,
                Email = user.Email,
                Role = Enum.IsDefined(request.Role) ? request.Role : user.Role,
                RequestType = UserRequestType.Edit,
                AuthStatus = AuthStatus.Pending,
                Initiator = username,
                InitiatorEmail = email,
                DateInitiated = DateTime.Now
            };

            await _pendingUserRepository.AddAsync(pendingUserRequest);

            return new Response<string>(pendingUserRequest.Id, message: $"Successfully edited user with username - {request.UserName}");
        }

        public async Task<Response<string>> DeleteUserAsync(DeleteUserRequest request)
        {
            var isUsernamePending = await _pendingUserRepository.IsUsernamePendingAsync(request.UserName);

            if (isUsernamePending)
            {
                return new Response<string>(null, message: $"The operation for the username '{request.UserName}' is awaiting approval.");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"Username '{request.UserName}' could not be found.");
            }

            string username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimFields.USER_NAME)?.Value;
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimFields.EMAIL)?.Value;

            PendingUser pendingUserRequest = new PendingUser()
            {
                UserName = request.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AuthStatus = AuthStatus.Pending,
                RequestType = UserRequestType.Delete,
                Initiator = username,
                InitiatorEmail = email,
                DateInitiated = DateTime.Now
            };

            await _pendingUserRepository.AddAsync(pendingUserRequest);

            return new Response<string>(pendingUserRequest.Id, message: $"Successfully deleted user with username - {request.UserName}");
        }

        public async Task<Response<string>> ResetUserAsync(ResetUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null || user.NormalizedEmail == request.Email.ToUpper())
            {
                throw new ApiException("Invalid username or email.");
            }

            string userName = user.UserName;
            string userFirstName = user.FirstName;
            string userEmail = user.Email;
            string token = Base64UrlEncoder.Encode (await _userManager.GeneratePasswordResetTokenAsync(user));

            var _Uri = new Uri(_apiResourceUrls.PasswordResetUrl);
            var resetUri = QueryHelpers.AddQueryString(_Uri.ToString(), "username", user.UserName);

            resetUri = QueryHelpers.AddQueryString(resetUri, "token", token);

            VerificationRequest verificationEmailRequest = new VerificationRequest()
            {
                FirstName = userFirstName,
                Url = resetUri
            };

            var emailRequest = new EmailRequest()
            {
                To = userEmail,
                Subject = "RESET PASSWORD",
                MailMessage = _templateService.GenerateHtmlStringFromViewsAsync("PasswordResetNotification", verificationEmailRequest)
            };

            string jobId = BackgroundJob.Enqueue<INotificationService>(x => x.SendEmail(emailRequest));
            _logger.LogInformation($"Successfully sent out the Password Reset job with Job ID {jobId}");

            return new Response<string>(user.Id, message: $"Successfully reset user with username - {request.UserName}");
        }


        public async Task<Response<string>> AuthorizeUserAsync(AuthorizeRequest request)
        {
            //string userRole = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
            string username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == UserClaimFields.EMAIL)?.Value;

            PendingUser pendingUserRequest = await _pendingUserRepository
                .GetFirstOrDefaultAsync(x => x.Id == request.Id && x.AuthStatus == AuthStatus.Pending);

            if (pendingUserRequest == null)
            {
                throw new ApiException($"No pending user request found for ID - {request.Id}.");
            }

            if (username == pendingUserRequest.Initiator && _applicationConfig.EnableSelfAuthCheck)
            {
                throw new ApiException($"Self authorization is not allowed.");
            }

            #region roleCheck
            // Check if the user has an "Admin" or "Authorizer" role
            //bool isAdminOrAuthorizer = userRole == "Admin" || userRole == "Authorizer";

            //if (!isAdminOrAuthorizer)
            //{
            //    throw new ApiException($"Only users with 'Admin' or 'Authorizer' role can authorize a user.");
            //} 
            #endregion

            if (request.AuthStatus == AuthorizersStatus.Approved)
            {
                switch (pendingUserRequest.RequestType)
                {
                    case UserRequestType.New:
                        await _AddUser(pendingUserRequest);
                        break;
                    case UserRequestType.Edit:
                        await _EditUser(pendingUserRequest);
                        break;
                    case UserRequestType.Delete:
                        await _DeleteUser(pendingUserRequest);
                        break;
                    default:
                        throw new ApiException($"Invalid request type by the initiator.");
                }
            }

            pendingUserRequest.Authorizer = username;
            pendingUserRequest.AuthorizerEmail = email;
            pendingUserRequest.DateAuthorized = DateTime.Now;
            pendingUserRequest.AuthStatus = request.AuthStatus == AuthorizersStatus.Approved ? AuthStatus.Approved : AuthStatus.Rejected;
            pendingUserRequest.AuthorizersComment = request.AuthorizersComment;

            await _pendingUserRepository.UpdateAsync(pendingUserRequest);

            // delete the pending user after authorization

            await _pendingUserRepository.DeleteAsync(pendingUserRequest);


            return new Response<string>(pendingUserRequest.Id, message: $"Successfully authorized user with Id - {request.Id}");
        }

        public async Task<Response<string>> ChangePasswordWithToken(ChangePasswordWithTokenRequest request)
        {
            User user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"No user found with username '{request.UserName}'.");
            }

            string token = HttpUtility.UrlDecode(request.Token);

            IdentityResult resetResponse = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!resetResponse.Succeeded)
            {
                throw new ApiException(resetResponse.Errors.FirstOrDefault().Description);
            }

            return new Response<string>(user.Id, message: $"Successfully reset password for user with username - {request.UserName}");
        }

        public async Task<Response<string>> ChangePassword(ChangePasswordRequest request)
        {
            User user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"No user found with username '{request.UserName}'.");
            }

            IdentityResult resetResponse = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.Password);

            if (!resetResponse.Succeeded)
            {
                throw new ApiException(resetResponse.Errors.FirstOrDefault().Description);
            }

            return new Response<string>(user.Id, message: $"Successfully reset password for user with username - {request.UserName}");
        }

        public async Task<Response<string>> PasswordReset(PasswordResetRequest request)
        {
            User user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"No user found with username '{request.UserName}'.");
            }

            string token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            IdentityResult resetResponse = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!resetResponse.Succeeded)
            {
                throw new ApiException(resetResponse.Errors.FirstOrDefault().Description);
            }

            return new Response<string>(user.Id, message: $"Successfully reset password for user with username - {request.UserName}");
        }

        /**
         * Private Functions for internal operations
         **/
        private async Task<User> _AddUser(PendingUser request)
        {
            // Map pending user to a new user
            User user = _mapper.Map<User>(request);

            // Set user properties
            user.IsActive = true;
            user.CreatedAt = DateTime.Now;
            user.LockoutEnabled = true;

            // Create the user
            IdentityResult result = await _userManager.CreateAsync(user);

            // If user creation is successful, proceed with additional steps
            if (result.Succeeded)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                //string encodedToken = HttpUtility.UrlEncode(token);
                string encodedToken = Base64UrlEncoder.Encode(token);

                var _endpointUri = new Uri(_apiResourceUrls.PasswordResetUrl);
                var addUserUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "username", user.UserName);

                addUserUri = QueryHelpers.AddQueryString(addUserUri, "token", encodedToken);

                //string url = $"{_apiResourceUrls.PasswordResetUrl}?token={encodedToken}&username={HttpUtility.UrlEncode(user.UserName)}";

                VerificationRequest verificationEmailRequest = new VerificationRequest()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    Url = addUserUri
                };

                // Prepare email request
                var emailRequest = new EmailRequest()
                {
                    To = user.Email,
                    Subject = "COMPLETE USER REGISTRATION",
                    MailMessage =  _templateService.GenerateHtmlStringFromViewsAsync("SetPasswordNotification", verificationEmailRequest)
                };

                // Schedule sending the registration email using Hangfire
                string jobId = BackgroundJob.Enqueue<INotificationService>(x => x.SendEmail(emailRequest));

                // Log information about the scheduled job
                _logger.LogInformation($"Successfully sent out the Password Reset job with Job ID {jobId}");

                return user;
            }
            else
            {
                throw new ApiException($"{result.Errors.FirstOrDefault()?.Description}");
            }
        }


        private async Task<User> _EditUser(PendingUser request)
        {
            User user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"No user found with username '{request.UserName}'.");
            }

            _mapper.Map(request, user);
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new ApiException($"{result.Errors.FirstOrDefault().Description}");
            }

            return user;
        }

        private async Task<User> _DeleteUser(PendingUser request)
        {
            User user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"The user with User Name - {request.UserName} does not exist.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new ApiException(result.Errors.FirstOrDefault().Description);
            }

            return user;
        }
    }
}

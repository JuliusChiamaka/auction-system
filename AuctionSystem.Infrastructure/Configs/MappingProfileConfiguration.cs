using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Service.DTO.Request;
using AuctionSystem.Service.DTO.Response;
using AutoMapper;
using System.Linq;

namespace AuctionSystem.Infrastructure.Configs
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            // USER MAPPINGS
            CreateMap<User, UserResponse>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.IsLoggedIn, opt => opt.MapFrom(src => src.IsLoggedIn))
                    .ForMember(dest => dest.LastLoginTime, opt => opt.MapFrom(src => src.LastLoginTime))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                    .ForMember(dest => dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd))
                    .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => src.LockoutEnabled))
                    .ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => src.AccessFailedCount))
                    .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<User, AuthenticationResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsLoggedIn, opt => opt.MapFrom(src => src.IsLoggedIn))
                .ForMember(dest => dest.LastLoginTime, opt => opt.MapFrom(src => src.LastLoginTime))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.LockoutEnd, opt => opt.MapFrom(src => src.LockoutEnd))
                .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForMember(dest => dest.AccessFailedCount, opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<PendingUser, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForAllOtherMembers(opts => opts.Ignore());


            CreateMap<AddUserRequest, PendingUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                //.ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<PendingUser, PendingUserResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.RequestType))
                .ForMember(dest => dest.AuthStatus, opt => opt.MapFrom(src => src.AuthStatus))
                .ForAllOtherMembers(opts => opts.Ignore());

            
        }
    }
}

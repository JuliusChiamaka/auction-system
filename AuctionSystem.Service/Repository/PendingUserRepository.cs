using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Repository
{
    public class PendingUserRepository : Repository<PendingUser>, IPendingUserRepository
    {
        public PendingUserRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<PendingUser>())
        {
        }

        public async Task<PagedList<PendingUser>> GetPendingUserListAsync(PendingUserQueryParameters queryParameters)
        {
            var whereList = new List<Expression<Func<PendingUser, bool>>>();
            var orderByList = new List<Tuple<SortingOption, Expression<Func<PendingUser, object>>>>();

            if (!string.IsNullOrEmpty(queryParameters.Query))
            {
                whereList.Add(p => p.Email.Contains(queryParameters.Query)
                    || p.FirstName.Contains(queryParameters.Query)
                    || p.Id.Contains(queryParameters.Query)
                    || p.LastName.Contains(queryParameters.Query)
                    || p.UserName.Contains(queryParameters.Query));
            }

            switch (queryParameters.Role)
            {
                case UserRole.Administrator:
                    whereList.Add(p => p.Role == UserRole.Administrator);
                    break;
                case UserRole.Authorizer:
                    whereList.Add(p => p.Role == UserRole.Authorizer);
                    break;
                case UserRole.Initiator:
                    whereList.Add(p => p.Role == UserRole.Initiator);
                    break;
                default:
                    break;
            }

            switch (queryParameters.RequestType)
            {
                case UserRequestType.New:
                    whereList.Add(p => p.RequestType == UserRequestType.New);
                    break;
                case UserRequestType.Edit:
                    whereList.Add(p => p.RequestType == UserRequestType.Edit);
                    break;
                case UserRequestType.Delete:
                    whereList.Add(p => p.RequestType == UserRequestType.Delete);
                    break;
                default:
                    break;
            }

            orderByList.Add(new Tuple<SortingOption, Expression<Func<PendingUser, object>>>(new SortingOption { Direction = SortingOption.SortingDirection.DESC }, p => p.CreatedAt));

            PagedList<PendingUser> response = await GetPagedListAsync(queryParameters, whereList, orderByList);

            return response;
        }

        public async Task<PendingUser> GetPendingUserByIdAsync(string UserId)
        {
            return await GetFirstOrDefaultAsync(x => x.Id == UserId.ToLower());
        }

        public async Task<bool> IsUsernamePendingAsync(string username)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);

                var pendingUser = await dbContext.PendingUser
                .FirstOrDefaultAsync(x => x.UserName == username && x.AuthStatus == AuthStatus.Pending);

                return pendingUser != null;
            }
               
        }
    }
}

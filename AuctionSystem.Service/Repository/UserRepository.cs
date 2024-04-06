using AuctionSystem.Domain.Auth;
using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.QueryParameters;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.Repository.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<User>())
        {
        }

        public async Task<PagedList<User>> GetUserListAsync(UserQueryParameters queryParameters)
        {
            var whereList = new List<Expression<Func<User, bool>>>();
            var orderByList = new List<Tuple<SortingOption, Expression<Func<User, object>>>>();

            if (!string.IsNullOrEmpty(queryParameters.Query))
            {
                whereList.Add(p => p.Email.Contains(queryParameters.Query)
                    || p.FirstName.Contains(queryParameters.Query)
                    || p.Id.Contains(queryParameters.Query)
                    || p.LastName.Contains(queryParameters.Query)
                    || p.PhoneNumber.Contains(queryParameters.Query)
                    || p.UserName.Contains(queryParameters.Query));
            }

            if (queryParameters.Status != 0)
            {
                switch (queryParameters.Status)
                {
                    case 1:
                        whereList.Add(p => p.IsActive);
                        break;
                    case 2:
                        whereList.Add(p => !p.IsActive);
                        break;
                    default:
                        break;
                }
            }

            orderByList.Add(new Tuple<SortingOption, Expression<Func<User, object>>>(new SortingOption { Direction = SortingOption.SortingDirection.DESC }, p => p.CreatedAt));

            PagedList<User> response = await GetPagedListAsync(queryParameters, whereList, orderByList);

            return response;
        }

        public async Task<User> GetUserByIdAsync(string UserId)
        {
            return await GetFirstOrDefaultAsync(x => x.Id == UserId.ToLower());
        }
    }
}


using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Contract.Base
{
    public interface IRepository<T> 
    {
        Task<T> GetByIdAsync(string id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<PagedList<T>> GetPagedListAsync(UrlQueryParameters queryParameters,
            List<Expression<Func<T, bool>>> predicate = null,
            List<Tuple<SortingOption, Expression<Func<T, object>>>> orderByList = null,
            string includeString = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true);
    }
}

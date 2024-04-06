using AuctionSystem.Domain.Common;
using AuctionSystem.Domain.Entities.Base;
using AuctionSystem.Persistence;
using AuctionSystem.Service.Contract.Base;
using AuctionSystem.Service.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuctionSystem.Service.Repository.Base
{
    public class Repository<T> : RepositoryBase, IRepository<T>
        where T : class
    {
        public Repository(IServiceScopeFactory serviceScopeFactory, Func<ApplicationDbContext, DbSet<T>> getDbSet)
        : base(serviceScopeFactory)
        {
            _getDbSet = getDbSet;
        }

        protected Func<ApplicationDbContext, DbSet<T>> _getDbSet { get; private set; }

        public async virtual Task<T> GetByIdAsync(string id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            return await _getDbSet(dbContext).FindAsync(id);
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                return await _getDbSet(dbContext).FindAsync(id);
            }
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            _getDbSet(dbContext).AddRange(entities);
            await dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            _getDbSet(dbContext).UpdateRange(entities);
            await dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task<T> AddAsync(T entity)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            var _entities = _getDbSet(dbContext);
            _entities.Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            var _entities = _getDbSet(dbContext);
            dbContext.Entry(entity).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            _getDbSet(dbContext).Remove(entity);

            await dbContext.SaveChangesAsync();
        }

        public async virtual Task<IReadOnlyList<T>> ListAllAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            return await _getDbSet(dbContext).ToListAsync();
        }

        public async virtual Task RefreshDb()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }

        public async Task<PagedList<T>> GetPagedListAsync(UrlQueryParameters queryParameters,
            List<Expression<Func<T, bool>>> predicate = null,
            List<Tuple<SortingOption, Expression<Func<T, object>>>> orderByList = null,
            string includeString = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            var _entities = _getDbSet(dbContext);

            var query = disableTracking ? _entities.AsNoTracking() : _entities;

            if (!string.IsNullOrWhiteSpace(includeString))
            {
                query = query.Include(includeString);
            }
            else if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderByList != null)
            {
                query = query.OrderBy(orderByList);
            }

            var totalCount = await query.CountAsync();

            var totalPages = totalCount / queryParameters.PageSize;

            if (totalCount % queryParameters.PageSize > 0)
                totalPages++;

            var items = totalCount > 0
                ? query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize).Take(queryParameters.PageSize).ToList()
                : new List<T>();

            return new PagedList<T>(items, totalCount, totalPages);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            var _entities = _getDbSet(dbContext);

            var query = disableTracking ? _entities.AsNoTracking() : _entities;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            bool disableTracking = true)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = GetDatabaseContext(scope);
            var _entities = _getDbSet(dbContext);

            var query = disableTracking ? _entities.AsNoTracking() : _entities;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).FirstOrDefaultAsync();
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}

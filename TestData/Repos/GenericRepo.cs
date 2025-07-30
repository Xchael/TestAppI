using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestData.Data;
using TestData.Repos.Interfaces;

namespace TestData.Repos
{
    public class GenericRepo<T>(TestAppIDbContext context) : IReadRepository<T>, IWriteRepository<T>
        where T : class
    {
        private readonly DbSet<T> _set = context.Set<T>();

        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _set.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _set.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity)
        {
            _set.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _set.UpdateRange(entities);
        }

        public void Remove(T entity)
        {
            _set.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _set.RemoveRange(entities);
        }

        public T GetById(int id)
        {
            return _set.Find(id)
                   ?? throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.");
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id)
                   ?? throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.");
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _set.Where(predicate).ToListAsync(cancellationToken);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _set.Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _set.AnyAsync(predicate, cancellationToken);
        }

        public T FindFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate)
                   ?? throw new KeyNotFoundException($"No entity of type {typeof(T).Name} matches the given predicate.");
        }

        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _set.FirstOrDefaultAsync(predicate, cancellationToken)
                   ?? throw new KeyNotFoundException($"No entity of type {typeof(T).Name} matches the given predicate.");
        }

        public List<T> GetAll()
        {
            return _set.ToList();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _set.ToListAsync(cancellationToken);
        }

        public PagedResult<T> GetPagedResult(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _set.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var totalCount = query.Count();
            if (totalCount == 0)
            {
                return new PagedResult<T>
                {
                    Queryable = Enumerable.Empty<T>().AsQueryable(),
                    RowCount = 0,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var pagedResult = new PagedResult<T>
            {
                Queryable = items.AsQueryable(),
                RowCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
            return pagedResult;
        }

        public async Task<PagedResult<T>> GetPagedResultAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _set.AsQueryable();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var totalCount = await query.CountAsync(cancellationToken);
            if (totalCount == 0)
            {
                return new PagedResult<T>
                {
                    Queryable = Enumerable.Empty<T>().AsQueryable(),
                    RowCount = 0,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            var pagedResult = new PagedResult<T>
            {
                Queryable = items.AsQueryable(),
                RowCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
            return pagedResult;
        }
    }
}

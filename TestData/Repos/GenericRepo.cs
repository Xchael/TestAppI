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
    // Generic repository class that provides basic CRUD operations for any entity type T.
    // T must be a class, allowing for flexibility in the types of entities managed.
    public class GenericRepo<T> : IReadRepository<T>, IWriteRepository<T>
        where T : class
    {
        private readonly DbSet<T> _set; // DbSet for the entity type T, allowing for database operations.

        // Constructor that initializes the repository with a database context.
        public GenericRepo(TestAppIDbContext context)
        {
            _set = context.Set<T>(); // Set<T> retrieves the DbSet for the specified entity type.
        }

        // Adds a new entity to the database.
        public void Add(T entity)
        {
            _set.Add(entity); // Adds the entity to the DbSet.
        }

        // Asynchronously adds a new entity to the database.
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken); // Asynchronously adds the entity to the DbSet.
        }

        // Adds a range of entities to the database.
        public void AddRange(IEnumerable<T> entities)
        {
            _set.AddRange(entities); // Adds multiple entities to the DbSet.
        }

        // Asynchronously adds a range of entities to the database.
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _set.AddRangeAsync(entities, cancellationToken); // Asynchronously adds multiple entities to the DbSet.
        }

        // Updates an existing entity in the database.
        public void Update(T entity)
        {
            _set.Update(entity); // Updates the specified entity in the DbSet.
        }

        // Updates a range of entities in the database.
        public void UpdateRange(IEnumerable<T> entities)
        {
            _set.UpdateRange(entities); // Updates multiple entities in the DbSet.
        }

        // Removes an entity from the database.
        public void Remove(T entity)
        {
            _set.Remove(entity); // Removes the specified entity from the DbSet.
        }

        // Removes a range of entities from the database.
        public void RemoveRange(IEnumerable<T> entities)
        {
            _set.RemoveRange(entities); // Removes multiple entities from the DbSet.
        }

        // Retrieves an entity by its ID.
        public T GetById(int id)
        {
            return _set.Find(id) // Finds the entity by ID.
                   ?? throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found."); // Throws an exception if not found.
        }

        // Asynchronously retrieves an entity by its ID.
        public async Task<T> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id) // Asynchronously finds the entity by ID.
                   ?? throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found."); // Throws an exception if not found.
        }

        // Finds entities that match a given predicate.
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate); // Returns entities that match the predicate.
        }

        // Asynchronously finds entities that match a given predicate.
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _set.Where(predicate).ToListAsync(cancellationToken); // Asynchronously returns matching entities.
        }

        // Checks if any entities exist that match a given predicate.
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _set.Any(predicate); // Returns true if any entities match the predicate.
        }

        // Asynchronously checks if any entities exist that match a given predicate.
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _set.AnyAsync(predicate, cancellationToken); // Asynchronously checks for matching entities.
        }

        // Finds the first entity that matches a given predicate or throws an exception if none found.
        public T FindFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate) // Finds the first matching entity.
                   ?? throw new KeyNotFoundException($"No entity of type {typeof(T).Name} matches the given predicate."); // Throws an exception if none found.
        }

        // Asynchronously finds the first entity that matches a given predicate or throws an exception if none found.
        public async Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _set.FirstOrDefaultAsync(predicate, cancellationToken) // Asynchronously finds the first matching entity.
                   ?? throw new KeyNotFoundException($"No entity of type {typeof(T).Name} matches the given predicate."); // Throws an exception if none found.
        }

        // Retrieves all entities from the database.
        public List<T> GetAll()
        {
            return _set.ToList(); // Returns all entities as a list.
        }

        // Asynchronously retrieves all entities from the database.
        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _set.ToListAsync(cancellationToken); // Asynchronously returns all entities as a list.
        }

        // Retrieves a paged result of entities based on the specified parameters.
        public PagedResult<T> GetPagedResult(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            if (page <= 0) page = 1; // Ensures the page number is at least 1.
            if (pageSize <= 0) pageSize = 10; // Ensures the page size is at least 10.

            var query = _set.AsQueryable(); // Converts the DbSet to an IQueryable for further querying.

            if (predicate != null)
            {
                query = query.Where(predicate); // Applies the predicate if provided.
            }

            var totalCount = query.Count(); // Gets the total count of entities.
            if (totalCount == 0)
            {
                return new PagedResult<T> // Returns an empty paged result if no entities are found.
                {
                    Queryable = Enumerable.Empty<T>().AsQueryable(),
                    RowCount = 0,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }

            if (orderBy != null)
            {
                query = orderBy(query); // Applies ordering if provided.
            }
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(); // Retrieves the specified page of entities.
            var pagedResult = new PagedResult<T>
            {
                Queryable = items.AsQueryable(),
                RowCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
            return pagedResult; // Returns the paged result.
        }

        // Asynchronously retrieves a paged result of entities based on the specified parameters.
        public async Task<PagedResult<T>> GetPagedResultAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default)
        {
            if (page <= 0) page = 1; // Ensures the page number is at least 1.
            if (pageSize <= 0) pageSize = 10; // Ensures the page size is at least 10.

            var query = _set.AsQueryable(); // Converts the DbSet to an IQueryable for further querying.

            if (predicate != null)
            {
                query = query.Where(predicate); // Applies the predicate if provided.
            }

            var totalCount = await query.CountAsync(cancellationToken); // Asynchronously gets the total count of entities.
            if (totalCount == 0)
            {
                return new PagedResult<T> // Returns an empty paged result if no entities are found.
                {
                    Queryable = Enumerable.Empty<T>().AsQueryable(),
                    RowCount = 0,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }

            if (orderBy != null)
            {
                query = orderBy(query); // Applies ordering if provided.
            }
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken); // Asynchronously retrieves the specified page of entities.
            var pagedResult = new PagedResult<T>
            {
                Queryable = items.AsQueryable(),
                RowCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
            return pagedResult; // Returns the paged result.
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Repos.Interfaces
{
    //Ienumerable This provides flexibility and hides the implementation details. It allows consumers to enumerate results but not to build further queries.
    //Use List<T> only for async methods that materialize results
    //Only use IQueryable<T> if you have a strong reason to let consumers build queries outside the repository.
    public interface IReadRepository<T> where T : class
    {
        // Retrieves an entity by its unique identifier.
        // The method is synchronous to allow immediate access to the entity.
        T GetById(int id);

        // Asynchronous version of GetById to support non-blocking calls.
        // Useful in scenarios where the calling thread should not be blocked.
        Task<T> GetByIdAsync(int id);

        // Finds entities that match the given predicate.   
        // Returns an IEnumerable<T> to allow for flexible enumeration without exposing IQueryable.
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        // Asynchronous version of Find to support non-blocking calls.
        // Accepts a CancellationToken to allow for cancellation of the operation.
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        // Checks if any entity exists that matches the given predicate.
        // Returns a boolean indicating existence, which is efficient for validation checks.
        bool Exists(Expression<Func<T, bool>> predicate);

        // Asynchronous version of Exists to support non-blocking calls.
        // Accepts a CancellationToken for operation cancellation.
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        // Finds the first entity that matches the given predicate or returns null if none found.
        // This method is useful for scenarios where only one result is needed.
        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);

        // Asynchronous version of FindFirstOrDefault for non-blocking calls.
        // Accepts a CancellationToken for operation cancellation.
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        // Retrieves all entities as a List<T>.
        // This method is synchronous and materializes the results for immediate use.
        List<T> GetAll();

        // Asynchronous version of GetAll to support non-blocking calls.
        // Accepts a CancellationToken for operation cancellation.
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        // Retrieves a paged result of entities based on the specified page and page size.
        // Allows for filtering and ordering through optional parameters.
        PagedResult<T> GetPagedResult(int page, int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        // Asynchronous version of GetPagedResult for non-blocking calls.
        // Accepts a CancellationToken for operation cancellation.
        Task<PagedResult<T>> GetPagedResultAsync(int page, int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default);
    }
}

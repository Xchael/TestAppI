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
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        bool Exists(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        List<T> GetAll();
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        PagedResult<T> GetPagedResult(int page, int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

        Task<PagedResult<T>> GetPagedResultAsync(int page, int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default);
    }
}

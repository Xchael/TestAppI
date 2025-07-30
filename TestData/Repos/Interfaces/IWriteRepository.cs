using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Repos.Interfaces
{
    //Ienumerable This provides flexibility and hides the implementation details. It allows consumers to enumerate results but not to build further queries.
    //Use List<T> only for async methods that materialize results
    //Only use IQueryable<T> if you have a strong reason to let consumers build queries outside the repository.
    public interface IWriteRepository<T> where T : class
    {
        void Add(T entity);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void AddRange(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}

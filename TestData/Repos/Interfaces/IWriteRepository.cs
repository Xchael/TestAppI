using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Repos.Interfaces
{
    // IEnumerable provides flexibility and hides implementation details.
    // Use List<T> only for async methods that materialize results.
    // Only use IQueryable<T> if you have a strong reason to let consumers build queries outside the repository.
    public interface IWriteRepository<T> where T : class
    {
        // Adds a single entity to the repository.
        // The method is synchronous to allow immediate execution.
        void Add(T entity);

        // Asynchronously adds a single entity to the repository.
        // This method allows for non-blocking operations, improving performance in UI applications.
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        // Adds a range of entities to the repository.
        // This method is synchronous to allow batch processing of entities.
        void AddRange(IEnumerable<T> entities);

        // Asynchronously adds a range of entities to the repository.
        // This method is designed for non-blocking operations, suitable for high-performance scenarios.
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        // Updates an existing entity in the repository.
        // The method is synchronous to ensure immediate consistency after the update.
        void Update(T entity);

        // Updates a range of entities in the repository.
        // This method is synchronous to allow batch updates for efficiency.
        void UpdateRange(IEnumerable<T> entities);

        // Removes a single entity from the repository.
        // The method is synchronous to ensure immediate removal.
        void Remove(T entity);

        // Removes a range of entities from the repository.
        // This method is synchronous to allow batch removal for efficiency.
        void RemoveRange(IEnumerable<T> entities);
    }
}

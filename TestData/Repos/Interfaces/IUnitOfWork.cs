using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Repos.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository method returns a tuple containing read and write repositories for a specific entity type.
        // The generic type T is constrained to class types, allowing for flexibility in repository usage.
        (IReadRepository<T> Read, IWriteRepository<T> Write) Repository<T>() where T : class;

        // Asynchronous method to save changes to the database, returning the number of affected rows.
        // It accepts a CancellationToken to allow for operation cancellation.
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // Synchronous method to save changes to the database, returning the number of affected rows.
        // This is useful for scenarios where asynchronous operations are not required.
        int SaveChanges();

        // Method to begin a transaction, allowing multiple operations to be executed as a single unit of work.
        void BeginTransaction();

        // Asynchronous method to begin a transaction, providing non-blocking behavior for transaction initiation.
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        // Method to commit the current transaction, ensuring all changes are saved to the database.
        void CommitTransaction();

        // Asynchronous method to commit the current transaction, allowing for non-blocking behavior.
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        // Method to rollback the current transaction, reverting any changes made during the transaction.
        void RollbackTransaction();

        // Asynchronous method to rollback the current transaction, providing non-blocking behavior for rollback operations.
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}

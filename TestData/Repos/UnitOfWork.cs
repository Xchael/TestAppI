using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestData.Data;
using TestData.Repos.Interfaces;

namespace TestData.Repos
{
    // The UnitOfWork class implements the IUnitOfWork interface and provides a way to manage database transactions and repositories.
    public class UnitOfWork(TestAppIDbContext context) : IUnitOfWork, IAsyncDisposable
    {
        private readonly ConcurrentDictionary<Type, object> _repo = new();

        // Dispose method to release unmanaged resources and dispose of the DbContext.
        public void Dispose()
        {
            context.Dispose();
        }

        // Asynchronous Dispose method to release unmanaged resources asynchronously.
        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        // Repository method to get the read and write repositories for a specific entity type.
        // The method is generic to allow for any class type to be used as a repository.
        public (IReadRepository<T> Read, IWriteRepository<T> Write) Repository<T>() where T : class
        {
            var type = typeof(T);

            // Check if the repository for the type already exists in the dictionary.
            if (!_repo.TryGetValue(type, out var repo))
            {
                // If not, create a new instance of GenericRepo for the type and store it.
                var genericRepo = new GenericRepo<T>(context);
                _repo[type] = genericRepo;
            }

            // Cast the stored repository to GenericRepo<T> and return it as both read and write repository.
            var casted = (GenericRepo<T>)_repo[type];
            return (casted, casted);
        }

        // Asynchronous method to save changes to the database, returning the number of affected rows.
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        // Synchronous method to save changes to the database, returning the number of affected rows.
        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        // Method to begin a database transaction.
        public void BeginTransaction()
        {
            context.Database.BeginTransaction();
        }

        // Asynchronous method to begin a database transaction.
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await context.Database.BeginTransactionAsync(cancellationToken);
        }

        // Method to commit the current database transaction.
        public void CommitTransaction()
        {
            context.Database.CommitTransaction();
        }

        // Asynchronous method to commit the current database transaction.
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await context.Database.CommitTransactionAsync(cancellationToken);
        }

        // Method to rollback the current database transaction.
        public void RollbackTransaction()
        {
            context.Database.RollbackTransaction();
        }

        // Asynchronous method to rollback the current database transaction.
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await context.Database.RollbackTransactionAsync(cancellationToken);
        }
    }
}

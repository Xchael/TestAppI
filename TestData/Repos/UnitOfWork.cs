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
    public class UnitOfWork(TestAppIDbContext context) : IUnitOfWork, IAsyncDisposable
    {
        private readonly ConcurrentDictionary<Type, object> _repo = new();

        public void Dispose()
        {
            context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
        }

        public (IReadRepository<T> Read, IWriteRepository<T> Write) Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repo.TryGetValue(type, out var repo))
            {
                var genericRepo = new GenericRepo<T>(context);
                _repo[type] = genericRepo;
            }

            var casted = (GenericRepo<T>)_repo[type];
            return (casted, casted);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public void BeginTransaction()
        {
            context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await context.Database.BeginTransactionAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            context.Database.CommitTransaction();
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await context.Database.CommitTransactionAsync(cancellationToken);
        }

        public void RollbackTransaction()
        {
            context.Database.RollbackTransaction();
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await context.Database.RollbackTransactionAsync(cancellationToken);
        }
    }
}

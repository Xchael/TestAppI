using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestData.Repos.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        (IReadRepository<T> Read, IWriteRepository<T> Write) Repository<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
        int SaveChanges();
        void BeginTransaction();
        Task BeginTransactionAsync(CancellationToken cancellationToken= default);
        void CommitTransaction();
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        void RollbackTransaction();
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}

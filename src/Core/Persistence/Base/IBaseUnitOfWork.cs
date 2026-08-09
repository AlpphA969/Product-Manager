using Domain.Base;

namespace Persistence.Base;

public interface IBaseUnitOfWork : IDisposable
{
    bool IsDisposed { get; }

    void SaveChanges();

    System.Threading.Tasks.Task SaveChangesAsync();

    Repository<T> GetRepository<T>() where T : BaseEntity;

    Task EnsureCreatedAsync();
}
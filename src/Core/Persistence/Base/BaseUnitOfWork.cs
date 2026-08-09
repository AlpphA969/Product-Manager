using Domain.Base;
using Microsoft.EntityFrameworkCore;
using Persistence.Tools;
using Persistence.Tools.Enums;

namespace Persistence.Base;

public abstract class BaseUnitOfWork : IBaseUnitOfWork
{
     protected BaseUnitOfWork(Options options) : base()
    {
        Options = options;
    }

    private Options Options { get; set; }

    private DatabaseContext? _databaseContext;

    internal DatabaseContext DatabaseContext
    {
        get
        {
            if (_databaseContext == null)
            {
                var optionsBuilder =
                    new DbContextOptionsBuilder<DatabaseContext>();

                switch (Options.Provider)
                {
                    case Provider.SqlServer:
                    {
                        optionsBuilder.UseSqlServer
                            (connectionString: Options.ConnectionString);

                        break;
                    }

                    case Provider.InMemory:
                    {
                        break;
                    }

                    default:
                    {
                        throw new NotSupportedException(nameof(Options.Provider));
                    }
                }

                _databaseContext =
                    new DatabaseContext(options: optionsBuilder.Options);
            }

            return _databaseContext;
        }
    }

    public bool IsDisposed { get; internal set; }

    public void SaveChanges()
    {
        DatabaseContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await DatabaseContext.SaveChangesAsync();
    }

    public Repository<T> GetRepository<T>() where T : BaseEntity
    {
        var result = new Repository<T>(DatabaseContext);

        return result;
    }

    public void Dispose()
    {
        Dispose(true);

        System.GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed == true)
        {
            return;
        }

        if (disposing == true)
        {
            if (_databaseContext != null)
            {
                _databaseContext.Dispose();
                _databaseContext = null;
            }
            
            IsDisposed = true;
        }
    }

    public async Task EnsureCreatedAsync()
    {
        await DatabaseContext.Database.EnsureCreatedAsync();
    }

    ~BaseUnitOfWork()
    {
        Dispose(false);
    }
}
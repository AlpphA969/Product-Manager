using Domain.Base;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstraction;

namespace Persistence;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    internal Repository(DatabaseContext databaseContext) : base()
    {
        DatabaseContext = databaseContext ?? throw new ArgumentNullException(paramName: nameof(databaseContext));
        DbSet = databaseContext.Set<T>();
    }

    internal DatabaseContext DatabaseContext { get; set; }

    private DbSet<T> DbSet { get; }


    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
                DbSet.Update(entity)
            , cancellationToken
        );
    }

    public virtual async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
                DbSet.Remove(entity)
            , cancellationToken
        );
    }

    public virtual async Task<bool> RemoveByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await FindByIdAsync(id, cancellationToken);

        if (entity != null)
        {
            await RemoveAsync(entity, cancellationToken);

            return true;
        }

        return false;
    }

    public virtual async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await DbSet
                .Where(element => element.Id == id)
                .FirstOrDefaultAsync(cancellationToken)
            ;

        return result;
    }

    public virtual async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await DbSet.ToListAsync(cancellationToken);

        return result;
    }

    public virtual async Task<List<T>> PaginationGet(int page, int pagesize, CancellationToken cancellationToken = default)
    {
        var result =await DbSet.OrderBy(x=>x.Id).Skip((page-1)*pagesize).Take(pagesize).ToListAsync(cancellationToken);
        return result;
    }

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var result = await DbSet.CountAsync(cancellationToken);
        return result;
    }
   

}
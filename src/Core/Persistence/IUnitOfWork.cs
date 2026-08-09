using Persistence.Abstraction;
using Persistence.Base;

namespace Persistence;

public interface IUnitOfWork : IBaseUnitOfWork
{
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
}
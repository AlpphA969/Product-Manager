using Persistence.Abstraction;
using Persistence.Base;
using Persistence.Repository;
using Persistence.Tools;

namespace Persistence;

public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
{
    public UnitOfWork(Options options) : base(options)
    {
    }

    private IProductRepository? _productRepository;

    public IProductRepository ProductRepository
    {
        get
        {
            if (_productRepository == null)
            {
                _productRepository = new ProductRepository(databaseContext: DatabaseContext);
            }

            return _productRepository;
        }
    }
    private ICategoryRepository? _categoryRepository;

    public ICategoryRepository CategoryRepository
    {
        get
        {
            if (_categoryRepository == null)
            {
                _categoryRepository = new CategoryRepository(databaseContext: DatabaseContext);
            }

            return _categoryRepository;
        }
    }
    private IProductCategoryRepository? _productCategoryRepository;

    public IProductCategoryRepository ProductCategoryRepository
    {
        get
        {
            if (_productCategoryRepository == null)
            {
                _productCategoryRepository = new ProductCategoryRepository(databaseContext: DatabaseContext);
            }

            return _productCategoryRepository;
        }
    }
}
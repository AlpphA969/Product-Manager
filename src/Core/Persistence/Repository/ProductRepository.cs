using Azure;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Models.ViewModel;
using Persistence.Abstraction;
namespace Persistence.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    internal ProductRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
        DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

        DbSet = databaseContext.Set<Product>();
        

    }
    internal DatabaseContext Databasecontext { get;  }
    private  DbSet<Product> DbSet { get; }

    public async Task<List<Product>> GetAllAsync(ProductFiltersViewModel query,int page , int pagesize, CancellationToken cancellationToken = default)
    {
        var productsQuery = DbSet
            .Where(x => query.name == null || x.Name.Contains(query.name))
            .Where(x => query.color == null || x.Color.Contains(query.color))
            .Where(x => query.MinPrice == null || x.Price >= query.MinPrice)
            .Where(x => query.MaxPrice == null || x.Price <= query.MaxPrice);

        if (query.categoriesId != null && query.categoriesId.Any())
        {
         productsQuery = productsQuery.Where(x => x.ProductCategories.Where(c => query.categoriesId.Contains(c.CategoryId)).Any());
        }
        // pagination
        if(page !=0 && pagesize !=0)
        {

            productsQuery = productsQuery.OrderBy(x => x.Id).Skip((page - 1) * pagesize).Take(pagesize);

        }


        var products = await productsQuery.ToListAsync(cancellationToken);


        return products;

    }

    

    public async Task PriceUpdateAsync(Product product)
    {
        await Task.Run(() => DbSet.Update(product));
       
        
    }

    public async Task UpdateInStockCountAsync(Product product)
    {
        await Task.Run(() => DbSet.Update(product));
    }
    public async Task<int> CountAsync(ProductFiltersViewModel query, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(x => query.name == null || x.Name.Contains(query.name))
            .Where(x => query.color == null || x.Color.Contains(query.color))
            .Where(x => query.MinPrice == null || x.Price >= query.MinPrice)
            .Where(x => query.MaxPrice == null || x.Price <= query.MaxPrice)
            .Where(x => query.categoriesId == null || !query.categoriesId.Any() ||
                        x.ProductCategories.Any(c => query.categoriesId.Contains(c.CategoryId)))
            .CountAsync(cancellationToken);
    }

}
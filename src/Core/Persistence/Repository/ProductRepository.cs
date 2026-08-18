using Azure;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Models.ViewModel;
using Persistence.Abstraction;
using System.Runtime.CompilerServices;
namespace Persistence.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    internal ProductRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
        DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

        DbSet = databaseContext.Set<Product>();


    }
    internal DatabaseContext Databasecontext { get; }
    private DbSet<Product> DbSet { get; }

    public async Task<PageDataModel<Product>> GetAllAsync(ProductFiltersViewModel query, int page, int pagesize, CancellationToken cancellationToken = default)
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
       



        var products = await productsQuery
            .OrderBy(x => x.Id)
            .ToPageDataAsync(pageSize: pagesize, pageNumber: page
            , cancellationToken);




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

    public async Task<List<ProductCategory>> GetAllProductCategoryAsync(Guid id)
    {
        var product = await FindByIdAsync(id);
        
        var result = product.ProductCategories;
        return result;
    }
}

public static class test
{
    public static async Task<PageDataModel<T>> ToPageDataAsync<T>(this IQueryable<T> queryable, 
        int pageSize,
        int pageNumber,
        CancellationToken cancellation = default) where T : Domain.Base.BaseEntity
    {

        var totalCount = await queryable.CountAsync(cancellation);

         queryable = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var data = await queryable.ToListAsync(cancellation);

        var pagedDataModel = new PageDataModel<T>(pageSize , totalCount , pageNumber , data)
        {
            data = data,
            TotalCount = totalCount,
            PageIndex = pageNumber,
            PageCount = (int)Math.Ceiling((double)totalCount / pageSize),
        };

        return pagedDataModel;
    }
}
using Domain.Entity;
using Models.ViewModel;
using Persistence.Repository;

namespace Persistence.Abstraction;

public interface IProductRepository : IRepository<Product>
{
    Task<PageDataModel<Product>> GetAllAsync(ProductFiltersViewModel query , int  page , int pagesie , CancellationToken cancellationToken = default);
    Task PriceUpdateAsync(Product product);
    Task UpdateInStockCountAsync(Product product);
   
    Task<List<ProductCategory>> GetAllProductCategoryAsync(Guid id); 



}
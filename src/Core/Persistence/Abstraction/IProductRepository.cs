using Domain.Entity;
using Models.ViewModel;

namespace Persistence.Abstraction;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllAsync(ProductFiltersViewModel query , int  page , int pagesie , CancellationToken cancellationToken = default);
    Task PriceUpdateAsync(Product product);
    Task UpdateInStockCountAsync(Product product);
    Task<int> CountAsync(ProductFiltersViewModel query, CancellationToken cancellationToken = default);




}
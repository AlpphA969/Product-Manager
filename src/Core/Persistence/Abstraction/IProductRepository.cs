using Domain.Entity;
using Models.ViewModel;

namespace Persistence.Abstraction;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllAsync(ProductFiltersViewModel query , CancellationToken cancellationToken = default);
    Task PriceUpdateAsync(Product product);
    Task UpdateInStockCountAsync(Product product);
    
    

}
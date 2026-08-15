using System.ComponentModel.Design;
using Domain.Entity;
using FluentResults;
using Models.ViewModel;
namespace Application.Abstraction;

public interface IProductService
{
    Task<Result<PageResultViewModel<ProductViewModel>>> GetAllProductsAsync(ProductFiltersViewModel query ,int page , int pagesize ,  CancellationToken cancellationToken = default);
    Task<Result> AddProductAsync(ProductViewModel productviewmodel);
    Task<Result<ProductViewModel>> FindByIdAsync(Guid id);
    Task<Result> UpdateAsync(ProductViewModel productviewmodel , Guid id );
    Task<Result> DeleteAsync(Guid id);
    Task<Result> UpdatePriceAsync( UpdatePriceViewModel updatePriceViewModel , Guid id );
    Task<Result> UpdateInStockCountAsync(UpdateInStockCountViewModel updateInStockCountViewModel, Guid id);
    Task<Result> AddCategoryToTheProduct(Guid id, List<string> categoriesid);
    Task<Result> DeleteProductCategoriesAsync(Guid id, List<string> categoriesid);
    Task<Result<PageResultViewModel<ProductViewModel>>> PaginationGet(int page, int pagesize);



}
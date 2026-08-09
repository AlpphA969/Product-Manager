
using Domain.Entity;
using FluentResults;
using Models.ViewModel;
namespace Application.Abstraction
{

    public interface ICategoryService
    {
        Task<Result> AddCategoryAsync(CategoryViewModel model);
        Task<Result> UpdateByNameAsync(CategoryViewModel model , Guid id );
        Task<Result> RemoveByIdAsync(Guid id);
         Task<Result> ActiveAsync(Guid id);
         Task<Result> InActiveAsync(Guid id);


    }
}

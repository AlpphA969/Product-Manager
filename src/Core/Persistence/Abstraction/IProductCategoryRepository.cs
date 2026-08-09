using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Abstraction
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<List<string>> FindCategoriesByProductIdAsync(Guid id);
        Task AddRangeAsync(List<ProductCategory> productCategories);
        Task DeleteRangeAsync(List<ProductCategory> categoriesid);
        Task<List<ProductCategory>> FindAllByProductIdAsync(Guid id);
    }
}

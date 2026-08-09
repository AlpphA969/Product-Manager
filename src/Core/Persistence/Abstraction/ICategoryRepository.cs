using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Abstraction
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> FindByNameAsync(string name);
        Task UpdateByNameAsync(Category category);
        Task<List<string>> FindByCategoriesId(List<string> categoriesid);
        Task<bool> AnyActiveAsync(List<string> categoriesid);
       
        



    }
}

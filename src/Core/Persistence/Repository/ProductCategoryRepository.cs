using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        internal ProductCategoryRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            Databasecontext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

            DbSet = databaseContext.Set<ProductCategory>();


        }

        internal DatabaseContext Databasecontext { get; }
        private DbSet<ProductCategory> DbSet { get; }

        public async Task AddRangeAsync(List<ProductCategory> productCategories)
        {
            await DbSet.AddRangeAsync(productCategories);   
        }

        public async Task DeleteRangeAsync(List<ProductCategory> categoriesid)
        {
            await Task.Run(() => DbSet.RemoveRange(categoriesid));
        }

        public async Task<List<ProductCategory>> FindAllByProductIdAsync(Guid id)
        {
            var result = await DbSet.Where(x => x.ProductId == id).ToListAsync();
            return result;
        }

        public async Task<List<string>> FindCategoriesByProductIdAsync(Guid id)
        {
            var result = await DbSet.Where(x=>x.ProductId==id).Select(x=>x.CategoryId).ToListAsync();
            return result.Select(x=>x.ToString()).ToList();
        }
    }
}
    
       

     


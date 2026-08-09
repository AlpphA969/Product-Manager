

using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstraction;

namespace Persistence
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        internal CategoryRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
            Databasecontext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));

            DbSet = databaseContext.Set<Category>();


        }

        internal DatabaseContext Databasecontext { get; }
        private DbSet<Category> DbSet { get; }

        public async Task<bool> AnyActiveAsync(List<string> categoriesid)
        {
            var result = await DbSet.Where(c => categoriesid.Contains(c.Id.ToString())).AnyAsync(c=>c.IsActive);
            return result;
        }

        public async Task<List<string>> FindByCategoriesId(List<string> categoriesid)
        {
            var result = await DbSet.Where(c => categoriesid.Contains(c.Id.ToString()))
                .Select(c => c.Id)
                .ToListAsync();
            return result.Select(x=>x.ToString()).ToList();
        }

        public async Task<Category> FindByNameAsync(string name)
        {
            var result = await DbSet.Where(x => x.Name == name).FirstOrDefaultAsync();
            return result;
        }

        public async Task UpdateByNameAsync(Category category)
        {
            await Task.Run(() => DbSet.Update(category));
            
            
        }
    }
}

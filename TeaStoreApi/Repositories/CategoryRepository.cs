using Microsoft.EntityFrameworkCore;
using TeaStoreApi.Data;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;

namespace TeaStoreApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApiDbContext dbContext;
        public CategoryRepository(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> AddCategory(Category category)
        {
            try
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await category.Image.CopyToAsync(fileStream);
                }

                category.ImageUrl = filePath.Substring(8);



                await dbContext.Categories.AddAsync(category);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
                if (existingCategory != null)
                {
                    dbContext.Categories.Remove(existingCategory);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await dbContext.Categories.ToListAsync();
        }
    }
}

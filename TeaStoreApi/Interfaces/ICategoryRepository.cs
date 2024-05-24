using TeaStoreApi.Models;

namespace TeaStoreApi.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<bool> AddCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}

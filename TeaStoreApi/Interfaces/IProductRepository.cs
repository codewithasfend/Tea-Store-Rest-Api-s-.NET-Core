using TeaStoreApi.Models;

namespace TeaStoreApi.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<object>> GetProducts(string productType, int? categoryId = null);
        Task<Product> GetProductById(int id);
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(int id, Product product);
        Task<bool> DeleteProduct(int id);   

    }
}

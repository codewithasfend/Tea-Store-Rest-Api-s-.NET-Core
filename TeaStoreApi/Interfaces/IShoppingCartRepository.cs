using TeaStoreApi.Models;

namespace TeaStoreApi.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<IEnumerable<object>> GetShoppingCartItems(int userId);
        Task<bool> AddToCart(ShoppingCartItem shoppingCartItem);
        Task<bool> UpdateCart(int productId, int userId , string action);
    }
}

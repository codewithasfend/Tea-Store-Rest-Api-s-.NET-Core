using TeaStoreApi.Models;

namespace TeaStoreApi.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<object>> GetAllOrders();
        Task<IEnumerable<object>> GetOrdersByUser(int userId);
        Task<IEnumerable<object>> GetOrderDetail(int orderId);
        Task<bool> PlaceOrder(Order order);
    }
}

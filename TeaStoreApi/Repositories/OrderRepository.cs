using Microsoft.EntityFrameworkCore;
using TeaStoreApi.Data;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;

namespace TeaStoreApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ApiDbContext dbContext;
        public OrderRepository(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<object>> GetAllOrders()
        {
            var orders = await dbContext.Orders.OrderByDescending(order => order.OrderPlaced).Select(order => new
            {
                Id = order.Id,
                OrderTotal = order.OrderTotal,
                OrderPlaced = order.OrderPlaced,
            }).ToListAsync();
            return orders;
        }

        public async Task<IEnumerable<object>> GetOrderDetail(int orderId)
        {


            var orderDetails = await (from orderDetail in dbContext.OrderDetails
                                      join product in dbContext.Products on orderDetail.ProductId equals product.Id
                                      where orderDetail.OrderId == orderId
                                      select new
                                      {
                                          Id = orderDetail.Id,
                                          Qty = orderDetail.Qty,
                                          SubTotal = orderDetail.TotalAmount,
                                          ProductName = product.Name,
                                          ProductImage = product.ImageUrl,
                                          ProductPrice = product.Price,
                                      }).ToListAsync();
            return orderDetails;
        }

        public async Task<IEnumerable<object>> GetOrdersByUser(int userId)
        {
            var orders = await dbContext.Orders.Where(order => order.UserId == userId).Select(order => new
            {
                Id = order.Id,
                OrderTotal = order.OrderTotal,
                OrderPlaced = order.OrderPlaced,
            }).ToListAsync();
            return orders;
        }

        public async Task<bool> PlaceOrder(Order order)
        {
            if (order == null)
            {
                return false;
            }
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync(); 

            var shoppingCartItems = await dbContext.ShoppingCartItems.Where(cart => cart.UserId == order.UserId).ToListAsync();
            foreach (var item in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Qty = item.Qty,
                    TotalAmount = item.TotalAmount,
                    OrderId = order.Id
                };
                await dbContext.OrderDetails.AddAsync(orderDetail);
            }
            // Remove the shopping cart items from the database
            dbContext.ShoppingCartItems.RemoveRange(shoppingCartItems);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}

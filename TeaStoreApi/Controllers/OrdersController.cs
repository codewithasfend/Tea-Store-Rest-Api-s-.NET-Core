using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;

namespace TeaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private IOrderRepository orderRepository;
        public OrdersController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await orderRepository.GetAllOrders();
            if (orders.Any())
            {
                return Ok(orders);
            }
            return NotFound();
        }

        // api/orders/user/1

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            var orders = await orderRepository.GetOrdersByUser(userId);
            if (orders.Any())
            {
                return Ok(orders);
            }
            return NotFound();
        }

        // api/orders/1

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var orderDetails = await orderRepository.GetOrderDetail(orderId);
            if (orderDetails.Any())
            {
                return Ok(orderDetails);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            var isAdded = await orderRepository.PlaceOrder(order);
            if (isAdded)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest();
        }


    }
}

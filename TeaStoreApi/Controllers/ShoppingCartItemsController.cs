using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;

namespace TeaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartItemsController : ControllerBase
    {
        private IShoppingCartRepository shoppingCartRepository;
        public ShoppingCartItemsController(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var shoppingCartItems = await shoppingCartRepository.GetShoppingCartItems(userId);
            if (shoppingCartItems.Any())
            {
                return Ok(shoppingCartItems);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ShoppingCartItem shoppingCartItem)
        {
            var isAdded = await shoppingCartRepository.AddToCart(shoppingCartItem);
            if (isAdded)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest("Something went wrong");
        }

        [HttpPut]
        public async Task<IActionResult> Put(int productId, int userId, string action)
        {
            var isUpdated = await shoppingCartRepository.UpdateCart(productId, userId, action);
            if (isUpdated)
            {
                return Ok("Cart updated");
            }
            return BadRequest("Something went wrong");
        }

    }
}

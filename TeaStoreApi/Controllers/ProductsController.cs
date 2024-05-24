using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;


namespace TeaStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }


        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> Get(string productType, int? categoryId = null)
        {
            var products = await productRepository.GetProducts(productType, categoryId);
            if (products.Any())
            {
                return Ok(products);
            }
            return NotFound();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await productRepository.GetProductById(id);
            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        // POST api/<ProductsController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Product product)
        {
            var isAdded = await productRepository.AddProduct(product);
            if (isAdded)
            {
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest("Something went wrong");
        }

        // PUT api/<ProductsController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] Product product)
        {
            var isUpdated = await productRepository.UpdateProduct(id, product);
            if (isUpdated)
            {
                return Ok("Record updated");
            }
            return BadRequest("Something went wrong");

        }

        // DELETE api/<ProductsController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await productRepository.DeleteProduct(id);
            if (isDeleted)
            {
                return Ok("Record has been deleted");
            }
            return BadRequest("Something went wrong");
        }
    }
}

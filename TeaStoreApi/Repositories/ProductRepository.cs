using Microsoft.EntityFrameworkCore;
using TeaStoreApi.Data;
using TeaStoreApi.Interfaces;
using TeaStoreApi.Models;

namespace TeaStoreApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ApiDbContext dbContext;
        public ProductRepository(ApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(fileStream);
                }

                product.ImageUrl = filePath.Substring(8);


                await dbContext.Products.AddAsync(product);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (existingProduct != null)
                {
                    dbContext.Products.Remove(existingProduct);
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

        public async Task<Product> GetProductById(int id)
        {
            return await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<object>> GetProducts(string productType, int? categoryId = null)
        {
            // return await dbContext.Products.ToListAsync();

            var productsQuery = dbContext.Products.AsQueryable();
            if (productType == "category" && categoryId != null)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
            }
            else if (productType == "bestselling")
            {
                productsQuery = productsQuery.Where(p => p.IsBestSelling == true);

            }
            else if (productType == "trending")
            {
                productsQuery = productsQuery.Where(p => p.IsTrending == true);

            }
            else
            {
                throw new ArgumentException("Invalid product type");
            }

            var products = await productsQuery.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            }).ToListAsync();

            return products;
        }

        public async Task<bool> UpdateProduct(int id, Product product)
        {
            try
            {
                var existingProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (existingProduct != null)
                {
                    var guid = Guid.NewGuid();
                    var filePath = Path.Combine("wwwroot", guid + ".jpg");
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.Image.CopyToAsync(fileStream);
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.Detail = product.Detail;
                    existingProduct.IsBestSelling = product.IsBestSelling;
                    existingProduct.IsTrending = product.IsTrending;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.ImageUrl = filePath.Substring(8);

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
    }
}

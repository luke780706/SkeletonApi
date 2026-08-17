using Microsoft.EntityFrameworkCore;
using SkeletonApi.Data;
using SkeletonApi.Models;

namespace SkeletonApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext db, ILogger<ProductService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _logger.LogInformation("Adding product: {ProductName}", product.Name);

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            _logger.LogInformation("Getting all products");

            return await _db.Products.ToListAsync(); 
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting product: {ProductId}", id);

            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                _logger.LogWarning("Product with id = {ProductId} was not found", id);
                return null;
            }

            return product;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            _logger.LogInformation("Updating product: {ProductName}", product.Name);

            var existingProduct = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (existingProduct == null)
            {
                _logger.LogWarning("Product with id = {ProductId} was not found", id);
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            await _db.SaveChangesAsync();

            return existingProduct;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting product with product id: {ProductId}", id);

            var productToDelete = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (productToDelete == null)
            {
                _logger.LogWarning("Product with id = {ProductId} was not found", id);
                return false;
            }

            _db.Products.Remove(productToDelete);
            await _db.SaveChangesAsync();

            return true;
        }

    }
}

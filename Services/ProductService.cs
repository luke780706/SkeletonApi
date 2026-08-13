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

        public Product AddProduct(Product product)
        {
            _logger.LogInformation("Adding product: {ProductName}", product.Name);

            _db.Products.Add(product);
            _db.SaveChanges();

            return product;
        }

        public IEnumerable<Product> GetAll()
        {
            _logger.LogInformation("Getting all products");

            return [.. _db.Products]; 
        }

        public Product? GetById(int id)
        {
            _logger.LogInformation("Getting product: {ProductId}", id);

            var product = _db.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                _logger.LogWarning("Product with id = {ProductId} was not found", id);
                return null;
            }

            return product;
        }

        public Product? Update(int id, Product product)
        {
            _logger.LogInformation("Updating product: {ProductName}", product.Name);

            var existingProduct = _db.Products.FirstOrDefault(x => x.Id == id);

            if (existingProduct == null)
            {
                _logger.LogWarning("Product with id = {ProductId} was not found", id);
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            _db.SaveChanges();

            return existingProduct;
        }

        public bool Delete(int id)
        {
            _logger.LogInformation("Deleting product with product id: {ProductId}", id);

            var productToDelete = _db.Products.FirstOrDefault(x => x.Id == id);

            if (productToDelete == null)
            {
                _logger.LogWarning("Product with id = {ProductId} was not found", id);
                return false;
            }

            _db.Products.Remove(productToDelete);
            _db.SaveChanges();

            return true;
        }

    }
}

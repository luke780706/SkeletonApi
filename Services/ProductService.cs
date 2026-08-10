using SkeletonApi.Data;
using SkeletonApi.Models;

namespace SkeletonApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public Product AddProduct(Product product)
        {

            _db.Products.Add(product);
            _db.SaveChanges();

            return product;
        }

        public IEnumerable<Product> GetAll()
        { 
            return [.. _db.Products]; 
        }

        public Product? GetById(int id)
        {
            return _db.Products.FirstOrDefault(x => x.Id == id);
        }

        public Product? Update(int id, Product product)
        {
            var existingProduct = _db.Products.FirstOrDefault(x => x.Id == id);

            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;

            _db.SaveChanges();

            return existingProduct;
        }

        public bool Delete(int id)
        {
            var productToDelete = _db.Products.FirstOrDefault(x => x.Id == id);

            if (productToDelete == null)
            {
                return false;
            }

            _db.Products.Remove(productToDelete);
            _db.SaveChanges();

            return true;
        }

    }
}

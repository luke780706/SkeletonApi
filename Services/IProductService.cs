using SkeletonApi.Models;

namespace SkeletonApi.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product AddProduct(Product product);
        Product? GetById(int id);
        Product? Update(int id, Product product);
        bool Delete(int id);
    }
}

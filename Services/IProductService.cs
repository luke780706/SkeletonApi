using SkeletonApi.Models;

namespace SkeletonApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> AddProductAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> UpdateAsync(int id, Product product);
        Task<bool> DeleteAsync(int id);
    }
}

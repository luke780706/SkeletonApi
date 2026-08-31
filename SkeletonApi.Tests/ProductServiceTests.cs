using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkeletonApi.Data;
using SkeletonApi.Models;
using SkeletonApi.Services;

namespace SkeletonApi.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext CreateAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;

            return new AppDbContext(options);
        }

        private ProductService CreateProductService(AppDbContext db)
        {
            var logger = new LoggerFactory().CreateLogger<ProductService>();
            return new ProductService(db, logger);
        }

        [Fact]
        public async Task GetByIdAsync_GivenProductExists_ThenReturnsProduct()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            db.Products.Add(new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10
            });

            await db.SaveChangesAsync();

            //When
            var result = await service.GetByIdAsync(1);

            //Then
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Product", result.Name);
            Assert.Equal(10, result.Price);
        }
        [Fact]
        public async Task GetByIdAsync_GivenProductDoesNotExist_ThenReturnsNull()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            //When
            var result = await service.GetByIdAsync(999);
            //Then
            Assert.Null(result);
        }
        [Fact]
        public async Task GetAllAsync_GivenProductsExist_ThenReturnsAllProducts()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);


            db.Products.AddRange(new Product { Id = 1, Name = "Product 1", Price = 10 },
                                 new Product { Id = 2, Name = "Product 2", Price = 20 });

            await db.SaveChangesAsync();

            //When
            var result = await service.GetAllAsync();

            //Then
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Id == 1);
            Assert.Contains(result, p => p.Id == 2);

        }
        [Fact]
        public async Task GetAllAsync_GivenNoProductsExist_ThenReturnsEmptyCollection()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            //When
            var result = await service.GetAllAsync();

            //Then
            Assert.Empty(result);
        }
        [Fact]
        public async Task AddProductAsync_GivenValidProduct_ThenAddsProduct()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            //When
            var result = await service.AddProductAsync(
                new CreateProductModel { Name = "Product 10", Price = 10 });

            //Then
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal("Product 10", result.Name);
            Assert.Equal(10, result.Price);

        }
        [Fact]
        public async Task UpdateAsync_GivenProductExists_ThenUpdatesProductPrice()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            db.Products.Add(
                new Product { Id = 10, Name = "Product 10", Price = 10 });

            await db.SaveChangesAsync();

            //When
            var result = await service.UpdateAsync(
                                        10, 
                                        new CreateProductModel 
                                        { 
                                            Name = "Product 10", Price = 20 
                                        });

            //Then
            Assert.NotNull(result);
            Assert.Equal(20, result.Price);
        }
        [Fact]
        public async Task UpdateAsync_GivenProductExists_ThenUpdatesProductName()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            db.Products.Add(
                new Product { Id = 10, Name = "Product 10", Price = 10 });

            await db.SaveChangesAsync();

            //When
            var result = await service.UpdateAsync(10,
                new CreateProductModel 
                { 
                    Name = "Product 20", Price = 10 
                });

            //Then
            Assert.NotNull(result);
            Assert.Equal("Product 20", result.Name);
        }
        [Fact]
        public async Task UpdateAsync_GivenProductDoesNotExist_ThenReturnsNull()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            //When
            var result = await service.UpdateAsync(20,
                new CreateProductModel 
                { 
                    Name = "Product 20", Price = 20 
                });

            //Then
            Assert.Null(result);
        }
        [Fact]
        public async Task DeleteAsync_GivenProductExists_ThenDeletesProduct()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            db.Add(
                new Product { Id = 10, Name = "Product 10", Price = 10 });

            await db.SaveChangesAsync();

            //When
            var result = await service.DeleteAsync(10);

            //Then

            var deletedProduct = await service.GetByIdAsync(10);

            Assert.True(result);
            Assert.Null(deletedProduct);
        }
        [Fact]
        public async Task DeleteAsync_GivenProductDoesNotExist_ThenReturnsFalse()
        {
            //Given
            var db = CreateAppDbContext();
            var service = CreateProductService(db);

            //When
            var result = await service.DeleteAsync(10);

            //Then
            Assert.False(result);
        }
    }
}

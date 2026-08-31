using Moq;
using SkeletonApi.Controllers;
using SkeletonApi.Models;
using SkeletonApi.Services;
using Microsoft.AspNetCore.Mvc;
using SkeletonApi.DTOs;

namespace SkeletonApi.Tests
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetById_ProductExists_ReturnOk()
        {
            //Given
            var mockService = new Mock<IProductService>();

            var product = new Product
            {
                Id = 10,
                Name = "Product 10",
                Price = 10
            };

            mockService.Setup(x => x.GetByIdAsync(10))
                                    .ReturnsAsync(product);

            var controller = new ProductsController(mockService.Object);
            //When
            var result = await controller.GetById(10);

            //Then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<ProductDto>(okResult.Value);

            Assert.Equal(10, returnedProduct.Id);
            Assert.Equal("Product 10", returnedProduct.Name);
            Assert.Equal(10, returnedProduct.Price);
        }

        [Fact]
        public async Task GetById_ProductDoesNotExist_ReturnsNotFound()
        {
            //Given
            var mockService = new Mock<IProductService>();

            mockService
                .Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((Product)null);

            var controller = new ProductsController(mockService.Object);

            //When
            var result = await controller.GetById(999);

            //Then
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task GetAll_ProductsExists_ReturnsProducts()
        {
            //Given
            var mockService = new Mock<IProductService>();

            mockService.Setup(x => x.GetAllAsync())
                                    .ReturnsAsync(
                                        (IEnumerable<Product>)[
                                            new Product { Id = 10, Name = "Product 10", Price = 10 },
                                            new Product { Id = 20, Name = "Product 20", Price = 20 }]);

            var controller = new ProductsController(mockService.Object);

            //When
            var result = await controller.GetAll();

            //Then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);

            Assert.Equal(2, products.Count());
        }
        [Fact]
        public async Task AddProduct_ValidProduct_ReturnsCreated()
        {
            //Given
            var mockService = new Mock<IProductService>();

            var createProductDto = new CreateProductDto
            {
                Name = "Product 10",
                Price = 10
            };

            var product = new Product
            {
                Id = 10,
                Name = "Product 10",
                Price = 10
            };

            mockService.Setup(x => x
                       .AddProductAsync(It.IsAny<CreateProductModel>()))
                       .ReturnsAsync(product);

            var controller = new ProductsController(mockService.Object);
            //When
            var result = await controller.AddProduct(createProductDto);

            //Then
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdProduct = Assert.IsType<ProductDto>(createdResult.Value);

            Assert.Equal(10, createdProduct.Id);
            Assert.Equal("Product 10", createdProduct.Name);
            Assert.Equal(10, createdProduct.Price);
        }
        [Fact]
        public async Task UpdateProduct_ProductExists_ReturnsOk()
        {
            //Given
            var mockServce = new Mock<IProductService>();

            Product changedProduct = new()
            {
                Id = 10,
                Name = "Product 20",
                Price = 20
            };

            UpdateProductDto productToChange = new()
            {
                Name = "Product 10",
                Price = 10
            };

            mockServce.Setup(x => x.UpdateAsync(10, It.IsAny<CreateProductModel>()))
                      .ReturnsAsync(changedProduct);

            var controller = new ProductsController(mockServce.Object);

            //When
            var result = await controller.UpdateProduct(10, productToChange);

            //Then
            var changeResult = Assert.IsType<OkObjectResult>(result.Result);
            var changedProductValue = Assert.IsType<ProductDto>(changeResult.Value);

            Assert.Equal("Product 20", changedProductValue.Name);
            Assert.Equal(20, changedProductValue.Price);
        }
        [Fact]
        public async Task UpdateProduct_ProductDoesNotExist_ReturnsNotFound()
        {
            //Given
            var mockService = new Mock<IProductService>();

            mockService.Setup(x => x.UpdateAsync(10, It.IsAny<CreateProductModel>()))
                                    .ReturnsAsync((Product?)null);

            var controller = new ProductsController(mockService.Object);

            UpdateProductDto productToChange = new()
            {
                Name = "Product 20",
                Price = 20
            };

            //When
            var result = await controller.UpdateProduct(10, productToChange);

            //Then
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task DeleteProduct_ProductExists_ReturnsNoContent()
        {
            //Given
            var mockService = new Mock<IProductService>();

            mockService.Setup(x => x.DeleteAsync(10))
                       .ReturnsAsync(true);

            var controller = new ProductsController(mockService.Object);

            //When
            var result = await controller.DeleteProduct(10);

            //Then
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task DeleteProduct_ProductDoesNotExist_ReturnsNotFound()
        {
            //Given
            var mockService = new Mock<IProductService>();

            mockService.Setup(x => x.DeleteAsync(10))
                       .ReturnsAsync(false);

            var controller = new ProductsController(mockService.Object);

            //When
            var result = await controller.DeleteProduct(10);

            //Then
            Assert.IsType<NotFoundResult>(result);
        }
    }
}